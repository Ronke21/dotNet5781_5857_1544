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
                case "2":
                //return new BLImp2();
                default:
                    return BLImp.Instance;
            }
        }
    }
}
