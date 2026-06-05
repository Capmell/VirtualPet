using System;
using System.Timers;
using Timer = System.Timers.Timer; // just having private static Timer didnt work but when I added this it worked 

namespace VirtualPetApp
{
    class Program
    {
        private static Pet myPet;
        private static Timer gameTimer;

        static void Main(string[] args)
        {
        
            
      
            myPet = Pet.LoadState();

            if (myPet == null)
            {
                Console.WriteLine("Welcome to your Virtual Pet! You don't have a pet yet.");
                Console.Write("Enter a name for your new pet: ");
                string name = Console.ReadLine();
               
                myPet = new Pet(name);
            }
            else
            {
                Console.WriteLine($"Loaded {myPet.Name}");
                //System.Threading.Thread.Sleep(1500);
            }
            // tried to slightly implement what we have been learning in class it kinda worked decided to remove it though
           
            gameTimer = new Timer(10000); 
            gameTimer.Elapsed += Tick;
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;

            bool running = true;
            while (running)
            {
                Menu2();
                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("1. Feed");
                Console.WriteLine("2. Play");
                Console.WriteLine("3. Save & Exit");
             

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        myPet.Feed();
                        break;
                    case "2":
                        myPet.Play();
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
            }

           
            gameTimer.Stop();
            myPet.SaveState();
            Console.WriteLine($"{myPet.Name} saved successfully.");
        }

        private static void Tick(Object source, ElapsedEventArgs e)
        {
            myPet.PassTime();
            Menu2();
            Console.Write("\n[Time Passed!] > ");
        }

        private static void Menu2()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine($" PET: {myPet.Name} | Growth: {myPet.GrowthStage} | Mood: {myPet.Mood}");
            Console.WriteLine("========================================");
            Console.WriteLine($" Hunger:    {myPet.Hunger}/100");
            Console.WriteLine($" Happiness: {myPet.Happiness}/100");
            Console.WriteLine($" Age Ticks: {myPet.AgeTicks}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(myPet.VisualRepresentation());
            Console.WriteLine("========================================");
        }
    }
}