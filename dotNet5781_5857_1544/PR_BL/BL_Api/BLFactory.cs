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
                    return new BLImp();
                case "2":
                //return new BLImp2();
                default:
                    return new BLImp();
            }
        }
    }
}
