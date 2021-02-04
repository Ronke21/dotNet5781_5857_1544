using BL;

namespace BLApi
{
    public static class BLFactory
    {
        public static IBL GetBL(string type)
        {
            switch (type)
            {
                case "1":
                    return BLImp.Instance;
                default:
                    return BLImp.Instance;
            }
        }
    }
}
