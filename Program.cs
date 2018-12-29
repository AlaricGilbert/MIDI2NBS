using NAudio.Midi;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;

namespace MIDIConventor
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] adapter = { 1, 3, 0, 4, 2 };
            /*
            var mf = new MidiFile("ADAMAS.mid", false);
            var interMidi = Midi2Intermediate.Convert(mf);
            string str = JsonConvert.SerializeObject(interMidi.Data);
            File.WriteAllText("mid.json", str);
            Console.ReadLine();*/
            string fname= Console.ReadLine();
            Midi conv = new Midi(fname);
            /*
            Midi2NBS conv = new Midi2NBS(fname);*/
            Console.WriteLine("Please input midi file name:");
            var fs = File.OpenWrite("midi2nbs.nbs");
            var bw = new BinaryWriter(fs);
            short length = 0;
            bw.Write(length);
            short height = conv.Layers;
            bw.Write(height);
            Console.WriteLine("Please input song name:");
            var sname = Console.ReadLine();
            WriteString(bw, sname);
            WriteString(bw, "Alaric's Midi2NBS");
            Console.WriteLine("Please input original author:");
            var oauthor = Console.ReadLine();
            WriteString(bw, oauthor);
            Console.WriteLine("Please input description:");
            var description = Console.ReadLine();
            WriteString(bw, description);
            short tempo = 1000;
            bw.Write(tempo);
            byte autosave = 1;
            bw.Write(autosave);
            byte autosave_duration = 10;
            bw.Write(autosave_duration);
            byte time_signature = 4;
            bw.Write(time_signature);

            bw.Write(0);//minutes spent
            bw.Write(0);//left clicks
            bw.Write(0);//right clicks
            bw.Write(0);//blocks added
            bw.Write(0);//blocks removed
            WriteString(bw, "ADAMAS.mid");
            
            short currTick = -1;
            for (short tick = 0; tick < conv.MaxNotePosition; tick++)
            {
                if (!conv.NoteData.ContainsKey(tick))
                    continue;
                short deltaTick = (short)(tick - currTick);
                bw.Write(deltaTick) ;

                for (int i = 0; i < conv.NoteData[tick].Count; i++)
                {
                    bw.Write((short)1);//jump down
                    byte inst = adapter[(int)conv.NoteData[tick][i].BaseType];
                    bw.Write(inst);
                    bw.Write((byte)(conv.NoteData[tick][i].Note+33));
                    //bw.Write((byte)BaseType.Piano);
                    //bw.Write((byte)conv.NoteData[tick][i].ActualNote);
                }
                bw.Write((short)0);//no more blocks

                currTick = tick;
            }
            bw.Write((short)0);//no more ticks

            for (int i = 0; i < height; i++)
            {
                WriteString(bw, $"layer {i}");
                bw.Write((byte)100);
            }
            bw.Write((byte)0);
            bw.Close();
            fs.Close();

            Console.ReadLine();
        }
        static void WriteString(BinaryWriter bw,string str)
        {
            bw.Write(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                bw.Write(str[i]);
            }
        }
    }
}
