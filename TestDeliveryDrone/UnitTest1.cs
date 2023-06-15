namespace test_drone_delivery
{


    [TestFixture]
    public class DroneDeliveryTests
    {
        private string inputFilePath = "input.txt";
        private string outputFilePath = "output.txt";

        [SetUp]
        public void SetUp()
        {
            // Create a sample input file
            string[] inputLines = new string[]
            {
                "[Drone1],[20], [Drone2 Beta], [250], [Drone3], [100]",
                "[LocationA], [200]",
                "[Locationb], [20]",
                "[Locationc], [10]",
                "[Locationd], [150]",
                "[Locatione], [220]",
                "[Locationf], [210]",
                "[Locationg], [20]",
                "[Locationh], [2]",
                "[Locationgx], [20]",
                "[Locationhs], [2]"
            };

            File.WriteAllLines(inputFilePath, inputLines);
        }

        [TearDown]
        public void TearDown()
        {
            // Delete the input and output files after each test
            if (File.Exists(inputFilePath))
                File.Delete(inputFilePath);

            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);
        }

        [Test]
        public void AssignDeliveries_WithValidInput_AssignsDeliveriesToDrones()
        {
            // Arrange
            DroneDelivery.Program.Main(null);

            // Act
            string[] outputLines = File.ReadAllLines(outputFilePath);

            // Assert
            string[] expectedOutputLines = new string[]
            {
                "[Drone1]",
                "Trip #1",
                "[Locationb]",
                "[Drone2 Beta]",
                "Trip #1",
                "[Locatione]",
                "Trip #2",
                "[Locationg]",
                "Trip #3",
                "[Locationc]",
                "[Drone3]",
                "Trip #1",
                "[Locationgx]",
                "Trip #2",
                "[Locationh]",
                "Trip #3",
                "[Locationhs]"
            };

            CollectionAssert.AreEqual(expectedOutputLines, outputLines);
        }

        [Test]
        public void AssignDeliveries_WithInvalidInput_ReturnsEmptyOutput()
        {
            // Arrange
            File.WriteAllText(inputFilePath, "Invalid input");

            // Act
            DroneDelivery.Program.Main(null);

            // Assert
            string[] outputLines = File.ReadAllLines(outputFilePath);
            Assert.IsEmpty(outputLines);
        }
    }
}