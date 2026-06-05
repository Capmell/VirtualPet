namespace PetTest
{
    using VirtualPet;
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void PassTime()
        {
           
            var pet = new Pet("TestPet") { Hunger = 50, Happiness = 50, Age = 0 };

           
            pet.PassTime();

            
            Assert.That(pet.Age, Is.EqualTo(1));
            Assert.That(pet.Hunger, Is.EqualTo(60));
            Assert.That(pet.Happiness, Is.EqualTo(45));
        }

        [Test]
        public void Hunger()
        {
           
            var pet = new Pet("TestPet") { Hunger = 95, Happiness = 5 };

         
            pet.PassTime();

         
            Assert.That(pet.Hunger, Is.EqualTo(100), "Hunger should max out at 100");
            Assert.That(pet.Happiness, Is.EqualTo(0), "Happiness should minimum out at 0");
        }

        [Test]
        public void Feed()
        {
          
            var pet = new Pet("TestPet") { Hunger = 20, Happiness = 95 };

            
            pet.Feed();

            Assert.That(pet.Hunger, Is.EqualTo(0), "Hunger cannot go below 0");
            Assert.That(pet.Happiness, Is.EqualTo(100), "Happiness cannot go above 100");
        }

        [Test]
        public void Play()
        {
           
            var pet = new Pet("TestPet") { Hunger = 85, Happiness = 50 };

            
            pet.Play();

           
            Assert.That(pet.Happiness, Is.EqualTo(35), "Happiness drops if played with while starving");
            Assert.That(pet.Hunger, Is.EqualTo(85), "Hunger shouldn't increase if play was rejected");
        }

        [Test]
        public void PlaySucceed()
        {
          
            var pet = new Pet("TestPet") { Hunger = 40, Happiness = 50 };

          
            pet.Play();

            Assert.That(pet.Happiness, Is.EqualTo(75));
            Assert.That(pet.Hunger, Is.EqualTo(50));
        }
    }
}
