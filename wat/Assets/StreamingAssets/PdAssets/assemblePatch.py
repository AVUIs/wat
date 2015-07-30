import math

width = 12
height = 8
prefix = "#X obj "

def ftom(fInHz):
	f_A4  = 440.0;
	return 69 + 12* math.log((fInHz/f_A4),2);

def mtof(midinote):
	f_A4  = 440.0;
	return f_A4 * math.pow(2.0,(midinote-69.0)/12.0);

with open('wgBank_gen.pd','w') as f:
	file.write(f,"#N canvas 570 171 987 900 10;")

midiLower = 4
midiScale = 1.01#1.0000001#2.0
midiScaleY = 1.01#1.0000001#1.2346#0.05

with open('wgBank_gen.pd','a') as f:
	for x in range(width):
		for y in range(height):
			freq = mtof((width-x)*midiScale+midiLower) * mtof((height-y)*midiScaleY)#+midiLower)
			period = 1000 / freq#(freq*freq)
			print freq
			pan = float(x) / width
			index = x + y*width
			gain = float(index) / (width*height)
			file.write( f, prefix + str(x*120) + " " + str(y*20) + " " + "wgUnit~ " + str(index) + " " + str(period) + " " + str(pan) + " " + str(gain) + ";")

