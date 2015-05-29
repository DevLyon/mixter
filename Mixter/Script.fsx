// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.


// Define your library scripting code here

#r "System.Speech"

open System.Speech.Synthesis

let synth = new SpeechSynthesizer()

synth.Speak("hello world")

