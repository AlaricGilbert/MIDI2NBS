﻿using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIConventor
{
    public class Midi2NBS // old version
    {
        public static int MinimumNote = NoteToInt("F#2");
        public static int MaximumNote = NoteToInt("F#8");
        public Dictionary<int, List<NoteInfo>> NoteData = new Dictionary<int, List<NoteInfo>>();
        public int MaxNotePosition { get; internal set; }
        public short Layers { get; internal set; }
        private MidiFile mf;
        public Midi2NBS(string path)
        {
            mf = new MidiFile(path, false);
            for (int i = 0; i < mf.Tracks; i++)
            {
                var @event = mf.Events[i];
                for (int j = 0; j < @event.Count; j++)
                {
                    var midiEvent = @event[j];
                    if (MidiEvent.IsNoteOn(midiEvent))
                    {

                        string name = ((NoteOnEvent)midiEvent).NoteName;
                        int note = NoteToInt(name);
                        if (note < MinimumNote || note > MaximumNote)
                        {
                            Console.WriteLine($"The key {name} is out of range!");
                            continue;
                        }
                        var info = IntToInfo(note);
                        int order = (int)(1 + midiEvent.AbsoluteTime * 4 / mf.DeltaTicksPerQuarterNote);
                        if (order > MaxNotePosition)
                            MaxNotePosition = order;
                        if (NoteData.ContainsKey(order))
                            NoteData[order].Add(info);
                        else
                            NoteData.Add(order, new List<NoteInfo> { info });
                        if (NoteData[order].Count > Layers) Layers = (short)NoteData[order].Count;
                    }
                }

            }
        }
        public static int NoteToInt(string name)
        {
            int note = (name[name.Length - 1] - 48) * 12;
            string name_ = name.Remove(name.Length - 1);
            switch (name_)
            {
                case "F#": note += 0; break;
                case "G": note += 1; break;
                case "G#": note += 2; break;
                case "A": note += 3; break;
                case "A#": note += 4; break;
                case "B": note += 5; break;
                case "C": note += -6; break;
                case "C#": note += -5; break;
                case "D": note += -4; break;
                case "D#": note += -3; break;
                case "E": note += -2; break;
                case "F": note += -1; break;
            }
            return note;
        }
        public static NoteInfo IntToInfo(int note)
        {
            NoteInfo info = new NoteInfo();
            int relativeNote = note - MinimumNote;
            info.BaseType = relativeNote >= 60 ? BaseType.SnareDrum : (BaseType)(relativeNote / 12);
            info.Note = relativeNote - (int)info.BaseType * 12;
            info.ActualNote = note - MinimumNote + 33;
            return info;
        }
    }
}
