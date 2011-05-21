/*
 Modelspoorgroep Venlo MGV136/84/81 Firmware

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
#include "servo.h"

// Current pulse width of each servo
volatile static unsigned char servoPulseWidth[4]; 
// Target pulse width of each servo
volatile static unsigned char servoPulseWidthTarget[4]; 
// Bitmask for adjusting pulse width of each servo
volatile static unsigned char servoAdjustMask[4]; 
#ifdef RELAY 
// Bitmask for adjusting pulse width of each servo
volatile static unsigned char servoRelayBits[4]; 
#endif
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
Wait for a given number of nano seconds
*/
static void WaitNS(unsigned int nanos) 
{
        nanos = 0xFFFF - nanos;
        
        // Program timer1
        TMR1L = nanos & 0xFF;
        TMR1H = nanos >> 8;
                
        // Enable timer
        TMR1IF = 0;
        TMR1ON = 1;
                
        // Wait until timer overflow
        while (!TMR1IF) { /* wait */ }
        TMR1ON = 0;
}

/*
Move the given servo to it's target position.
*/
void MoveServo(unsigned char index)
{
	static unsigned int tmp;
	static unsigned char servoBit;
	static unsigned char pulseWidth; // Width of pulse in 10us
	static unsigned char pulseWidthTarget;
	static unsigned char adjustMask;
	static unsigned char servoStateIndex;
#ifdef RELAY
	static unsigned char relayBits;
#endif

#ifdef RelayStart 
	RelayStart(index, relayBits);
#ifdef RelayUpdate
	RelayUpdate();
#endif
#endif

	// Initialize
	servoBit = CLK_BIT(index);	
	pulseWidth = servoPulseWidth[index];
	pulseWidthTarget = servoPulseWidthTarget[index];
	adjustMask = servoAdjustMask[index];
#ifdef RELAY
	relayBits = servoRelayBits[index];
#endif	
	
	while (pulseWidth != pulseWidthTarget)
	{
		// Calculate length of pulse in ns
		tmp = pulseWidth;
		tmp *= 10;
		//tmp += 750;

		// Start pulse
		OUTPUT |= servoBit;

		// Wait until end of pulse
		WaitNS(tmp);
		
		// End pulse
		OUTPUT ^= servoBit;
		
		// Calculate delay
		tmp = 5000 - tmp; // 5ms - pulse-width (1..2ms)
		WaitNS(tmp);

		// Middle reached?
#ifdef RelayMiddle
		if (pulseWidth == 150) 
		{
			RelayMiddle(index, relayBits);
#ifdef RelayUpdate
			RelayUpdate();
#endif
		}
#endif			
		
		// Update pulse width
		if (pulseWidth < pulseWidthTarget) 
		{
			pulseWidth++;
		}
		else 
		{
			pulseWidth--;
		}
	}

	// Save new state
	servoPulseWidth[index] = pulseWidth;

	// Target reached
#ifdef RELAY
	RelayEnd(index, relayBits);
#ifdef RelayUpdate
	RelayUpdate();
#endif
#endif
}

/*
Setup servo settings
*/
void SetupServos() 
{
	unsigned char i;
	
	// Initialize state variables
#ifdef RELAY
	allRelayBits = 0;
#endif
	for (i = 0; i != 4; i++) 
	{
		// Start in the middle position
		servoPulseWidth[i] = 149;
		servoPulseWidthTarget[i] = 150;
		servoAdjustMask[i] = 0x03;
#ifdef RELAY
		servoRelayBits[i] = 0;
#endif
	}
	PORTB = 0;
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
	// Set state info
	servoPulseWidthTarget[index] = pulseWidthTarget;
#ifdef RELAY
	servoRelayBits[index] = relayBits;
#endif
}

/*
Change the change mode of the servo
Servo: 0,1,2,3
adjustMask: 0,1,2,3
*/
void SetServoAdjust(unsigned char index, unsigned char adjustMask)
{
	// Update
	servoAdjustMask[index] = adjustMask;
}

#ifdef FEEDBACK
/*
* Update the feedback bits based on the current input bits
*/
void UpdateFeedbacks(unsigned char input) 
{
	FEEDBACK1 = (input & 0x01); 
	input >>= 1;
	FEEDBACK2 = (input & 0x01); 
	input >>= 1;
	FEEDBACK3 = (input & 0x01); 
	input >>= 1;
	FEEDBACK4 = (input & 0x01); 
}
#endif
