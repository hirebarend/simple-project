namespace SimpleProject.Shared.Misc
{
    public static class ChaosMonkey
    {
        private static uint _count = 0;

        private static Random _random = new Random();

        public static void Do()
        {
            _count += 1;

            //if (_count % 7 == 0)
            //{
            //    throw new Exception($"ChaosMonkey.Do");
            //}

            //if (_random.NextInt64(0, 1000) < 200)
            //{
            //    throw new Exception($"ChaosMonkey.Do");
            //}
        }
    }
}
