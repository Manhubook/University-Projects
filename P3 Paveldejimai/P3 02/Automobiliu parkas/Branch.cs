namespace Automobiliu_parkas
{
    class Branch
    {
        public string Town { get; set; }
        public string Address { get; set; }
        public string TelNumber { get; set; }
        public CarContainer krovCar { get; private set; }
        public CarContainer lengCar { get; private set; }
        public CarContainer autoCar { get; private set; }
        public CarContainer allCars;
        public Branch(string town)
        {
            Town = town;
            krovCar = new CarContainer(Program.CMaxCars);
            lengCar = new CarContainer(Program.CMaxCars);
            autoCar = new CarContainer(Program.CMaxCars);
            allCars = new CarContainer(Program.CMaxCars * Program.CMaxBranches);
        }
    }
}
