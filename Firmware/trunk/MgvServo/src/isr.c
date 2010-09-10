/*
 Modelspoorgroep Venlo MGV81 Firmware

 Copyright (C) Ewout Prangsma <ewout@prangsma.net>

 This program is free software; you can redistribute it and/or
 modify it under the terms of the GNU General Public License
 as published by the Free Software Foundation; either version 2
 of the License, or (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

#include "device.h"
#include "isr.h"

volatile static unsigned char isrState;
volatile static ServoState servoStates[4];
volatile static ServoState* servoStatePtr;
static unsigned char servoBit;
#ifdef FEEDBACK
static unsigned char feedbackMask;
#endif
static unsigned char zero;
#ifdef RELAY 
static unsigned char allRelayBits;
static unsigned char relayTmp;
static unsigned char relayMasks[] = { 0x03, 0x0C, 0x30, 0xC0 };
#endif

#ifdef SPEED
	#define ADJUST_BIT(state) (1 << (((state) >> 3) & 0x03))
#else
	#define ADJUST_BIT(state) 0x05 /* Half speed */
#endif

/*
Interrupt service routine.
This function is called twice per 5ms.
The first time is starts the pulse for a servo, the second time it ends the pulse for a servo.
*/
void ISR() interrupt 0
{
	static unsigned char pulseWidth; // Width of pulse in 10us
	static unsigned char pulseWidthTarget;
	static unsigned char adjustMask;
	static UU16 tmr1;	
	static unsigned int tmp;
	static ServoState* tmpStatePtr;
	
	// Test if interrupt comes from TMR1
	if (!TMR1IF) 
	{ 
		return; 
	}
	
	pulseWidth = servoStatePtr->pulseWidth;
	pulseWidthTarget = servoStatePtr->pulseWidthTarget;
	adjustMask = servoStatePtr->adjustMask;
	tmp = pulseWidth;
	tmp = (tmp << 3) + (tmp << 1); // Fast implementation of * 10

	// Start with time taken since interrupt
	tmr1.U8[MSB] = TMR1H;
	tmr1.U8[LSB] = TMR1L;

	if ((isrState & 0x01) == 0) 
	{
		// Next timer is at 0xFFFF - (pulseWidth * 10)
		// Subtract pulseWidth*10
		tmr1.U16 += 79; // Compensation needed to get exact timing
		tmr1.U16 -= tmp;
		// Reprogram timer
		TMR1H = tmr1.U8[MSB];
		TMR1L = tmr1.U8[LSB];

		// Start pulse
		if (pulseWidth != pulseWidthTarget) 
		{
			// Only start pulse if position is not reached
			OUTPUT |= servoBit;
		} 
		else 
		{
			OUTPUT |= servoBit;
			//OUTPUT |= zero;
		}		
	}
	else 
	{
		// Next timer is at 0xFFFF - (5000 + (pulseWidth * 10))
		// Add 5000
		tmr1.U16 -= (5000 - 88); // 88 is compensation needed to get exact timing
		tmr1.U16 += tmp;
		// Reprogram timer
		TMR1H = tmr1.U8[MSB];
		TMR1L = tmr1.U8[LSB];

		// End pulse
		OUTPUT &= ~servoBit;
		
		// Update pulse width
		if (pulseWidth == pulseWidthTarget) 
		{
			// Target reached
#ifdef FEEDBACK
			feedbackMask |= servoBit;
#endif
#ifdef RELAY
			// Hack to workaround compiler bug wrt 3rd argument of _getptr1
			servoStatePtr->pulseWidth = pulseWidth;
			RelayEnd(((isrState >> 1) & 0x03), servoStatePtr->relayBits);
#endif
		}
		else
		{
			// Only adjust when bit in adjust mask is set.
			if ((ADJUST_BIT(isrState) & adjustMask) != 0) 
			{
				if (pulseWidth < pulseWidthTarget) 
				{
					servoStatePtr->pulseWidth = pulseWidth + 1;
				}
				else 
				{
					servoStatePtr->pulseWidth = pulseWidth - 1;
				}
			}
#ifdef RelayMiddle
			if (pulseWidth == 150) 
			{
				RelayMiddle(((isrState >> 1) & 0x03), servoStatePtr->relayBits);
			}
#endif			
		}
		
		// Move to next servoState
		servoBit <<= 1;
		servoStatePtr++;
	}
	
	// Update state
	if ((isrState & 0x07) == 7) 
	{ 
#ifdef RelayUpdate
		RelayUpdate();
#endif
	
		// Go back to servo 1
		servoBit = CLK_BIT(0);
		servoStatePtr = &servoStates[0];
	}
	
	// Next state
	isrState++; 

	TMR1IF = 0; // Reset timer1 flag
}

/*
Setup timer settings
*/
void SetupTimer() 
{
	unsigned char i;
	
	// Initialize state variables
	isrState = 0;
	servoBit = CLK_BIT(0);
#ifdef RELAY
	allRelayBits = 0;
#endif
	zero = 0;
	for (i = 0; i != 4; i++) 
	{
		// Start in the middle position
		servoStates[i].pulseWidth = 149;
		servoStates[i].pulseWidthTarget = 150;
		servoStates[i].adjustMask = 0x03;
#ifdef RELAY
		servoStates[i].relayBits = 0;
#endif
	}
	servoStatePtr = &servoStates[0];

	PORTB = 0;
	TMR1CS = 0;	// Use internal clock
	TMR1H = 0xFF;	// First interrupt when 0xFFFF is eached
	TMR1L = 0xF0;  //
	TMR1IF = 0;	// Reset Timer1 flag
	TMR1IE = 1;	// Enable interrupt
	TMR1ON = 1;	// Enables timer1
	PEIE = 1;	// Enable peripheral interrupts
}

/*
Change the target of the servo
Servo: 0,1,2,3
adjustMask: 3 bits
pulseWidthTarget: 50..250
relayBits: exactly 1 bit must be set. 
*/
#ifdef RELAY
void SetServoTarget(unsigned char index, unsigned char pulseWidthTarget, unsigned char relayBits)
#else
void SetServoTarget(unsigned char index, unsigned char pulseWidthTarget)
#endif
{
	ServoState* state;

	// Get pointer to state info 
	state = &servoStates[index];
	
	// Turn interrupts off
	GIE = 0;

	// Set state info
	state->pulseWidthTarget = pulseWidthTarget;
#ifdef RELAY
	state->relayBits = relayBits;
#endif
#ifdef RelayStart 
	RelayStart(index, relayBits);
#endif

#ifdef FEEDBACK
	// Turn off passing input state to feedback
	feedbackMask &= ~CLK_BIT(index);
#endif

	// Turn interrupts on
	GIE = 1; 
}

/*
Change the change mode of the servo
Servo: 0,1,2,3
adjustMask: 0,1,2,3
*/
void SetServoAdjust(unsigned char index, unsigned char adjustMask)
{
	ServoState* state;

	// Get pointer to state info 
	state = &servoStates[index];

	// Turn interrupts off
	GIE = 0;

	// Update
	state->adjustMask = adjustMask;
	
	// Turn interrupts on
	GIE = 1; 
}

#ifdef FEEDBACK
/*
* Update the feedback bits based on the current input bits
*/
void UpdateFeedbacks(unsigned char input) 
{
	if (feedbackMask & CLK_BIT(0)) { FEEDBACK1 = (input & 0x01); }
	input >>= 1;
	if (feedbackMask & CLK_BIT(1)) { FEEDBACK2 = (input & 0x01); }
	input >>= 1;
	if (feedbackMask & CLK_BIT(2)) { FEEDBACK3 = (input & 0x01); }
	input >>= 1;
	if (feedbackMask & CLK_BIT(3)) { FEEDBACK4 = (input & 0x01); }
}
#endif