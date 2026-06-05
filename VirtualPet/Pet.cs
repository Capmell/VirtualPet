using System;
using System.IO;
using System.Text.Json;

namespace VirtualPetApp
{
    public class Pet
    {
       
        public string Name { get; set; }
        public int Hunger { get; set; } 
        public int Happiness { get; set; } 
        public int AgeTicks { get; set; } 

        
        private const int YouthMax = 5;
        private const int AdultMax = 15;

        private const string SaveFileName = "pet_state.json";

        public Pet(string name)
        {
            Name = name;
            Hunger = 50; 
            Happiness = 50;
            AgeTicks = 0;
        }

        
        public string GrowthStage
        {
            get
            {
                if (AgeTicks < YouthMax) return "Baby";
                if (AgeTicks < AdultMax) return "Child";
                return "Adult";
            }
        }

      
        public string Mood
        {
            get
            {
                if (Hunger > 75) return "Starving";
                if (Happiness < 30) return "Depressed";
                if (Happiness > 70 && Hunger < 40) return "Joyful";
                return "normal";
            }
        }

       
        public void PassTime()
        {
            AgeTicks++;
            Hunger = Math.Min(100, Hunger + 10); 
            Happiness = Math.Max(0, Happiness - 5); 
        }

   
        public void Feed()
        {
            Hunger = Math.Max(0, Hunger - 30);
            Happiness = Math.Min(100, Happiness + 10);
        }

        public void Play()
        {
            if (Hunger > 80)
            {
               
                Happiness = Math.Max(0, Happiness - 15);
                return;
            }
            Happiness = Math.Min(100, Happiness + 25);
            Hunger = Math.Min(100, Hunger + 10); 
        }

  
        // Im better with file .io but I decided to learn the json string way because it ended up working better
       
        public void SaveState()
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SaveFileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save pet: {ex.Message}");
            }
        }

        public static Pet LoadState()
        {
            if (!File.Exists(SaveFileName)) return null;

            try
            {
                string jsonString = File.ReadAllText(SaveFileName);
                return JsonSerializer.Deserialize<Pet>(jsonString);
            }
            catch
            {
                return null; 
            }
        }

        
        public string VisualRepresentation()
        {
            
            string stage = GrowthStage;
            string mood = Mood;

            if (stage == "Baby")
            {
                if (mood == "Starving" || mood == "Depressed") return " :(";
                if (mood == "Joyful") return "  :)";
                return " :|"; 
            }
            else if (stage == "Child")
            {
                if (mood == "Starving" || mood == "Depressed") return " :(";
                if (mood == "Joyful") return " :)";
                return " :|";
            }
            else 
            {
                if (mood == "Starving" || mood == "Depressed") return " :(  ";
                if (mood == "Joyful") return " :)";
                return " :|";
            }
        }
    }
}