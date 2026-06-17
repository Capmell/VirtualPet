using System;
using System.Text;
using System.Text.Json;
using System.Timers;
using Timer = System.Timers.Timer; // just having private static Timer didnt work but when I added this it worked 

namespace VirtualPet
{
    class Program
    {
        private static Pet myPet;
        private static Timer gameTimer;
        private static readonly HttpClient client = new HttpClient();
        private const string BaseApiUrl = "https://virtualpetparty.up.railway.app/api/room";

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
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Feed");
                Console.WriteLine("2. Play");
                Console.WriteLine("3. create party room");
                Console.WriteLine("4. join party room");
                Console.WriteLine("5. Save and Exit");


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
                        CreatePartyRoom();
                        break;
                    case "4":
                        JoinPartyRoom();
                        break;
                    case "5":
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
            Console.WriteLine($" Age: {myPet.Age}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(myPet.VisualRepresentation());
            Console.WriteLine("========================================");
        }

        // did some  extra research into the json stuff to keep it consistent as well as encoding since im using the console
        public static async Task CreatePartyRoom()
        {
            Console.Clear();
            Console.WriteLine("Creating a pet party room online...");

            var payload = new
            {
                name = myPet.Name,
                image = myPet.GetBase64Image()
            };

            try
            {
                string jsonPayload = JsonSerializer.Serialize(payload);
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{BaseApiUrl}/create", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

                    Console.WriteLine("\n=== ROOM CREATED SUCCESSFULLY ===");
                    Console.WriteLine($"Server Response: {responseBody}");
                    Console.WriteLine("\nGive this Room ID/code to a friend so they can join you!");
                }
                else
                {
                    Console.WriteLine($"Failed to host room. Server returned: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to the game menu.");
            Console.ReadKey();
        }


        private static async Task JoinPartyRoom()
        {
            Console.Clear();
            Console.Write("Enter the Room ID you want to join: ");
            string roomId = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(roomId) || roomId.Length != 6)
            {
                Console.WriteLine("Invalid room ID layout. Must be exactly 6 characters.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Connecting to room {roomId}...");




            HttpContent emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{BaseApiUrl}/join/{roomId}", emptyContent);



            string jsonResponse = await response.Content.ReadAsStringAsync();


            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("visitor", out JsonElement visitorElement))
                {
                    string visitorName = visitorElement.GetProperty("name").GetString();
                    string visitorB64Image = visitorElement.GetProperty("image").GetString();



                    Console.Clear();
                    Console.WriteLine($"   in party   ");
                    Console.WriteLine($"  you met another pet named: {visitorName}!");



                }
            }



            Console.WriteLine("\nPress any key to return to the game menu.");
            Console.ReadKey();
        }
    }
}


            
        


    

    
