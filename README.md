# SimpleSynth
A basic 3-osc audio synthesizer built on the NAudio framework for .NET. I wrote this as a class project for Windows App Development and as a way to familiarize myself with digital audio synthesis. 

I generate my basic tones from sine, square, triangle, and sawtooth waveforms. I then arrange these into individual oscillators that I combine to create a unique sound. The notes can be played by either right-clicking on the built in keyboard or by using the preset key-bindings assigned to each note. I also included an experimental ADSR envelope and lowpass filter, but both of these had some issues. I'll try to come back to this project in the future, but I may opt to rewrite it with a different core library.
