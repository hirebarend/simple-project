namespace SimpleProject.Shared.Misc
{
    public static class ChaosMonkey
    {
        private static uint _count = 0;

        public static void Do()
        {
            _count += 1;

            if (_count % 3 == 0)
            {
                throw new Exception($"ChaosMonkey.Do");
            }
        }
    }
}
