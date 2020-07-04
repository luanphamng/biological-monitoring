
namespace BIO_MONITORING
{
    public class Constant
    {

        public static string EncodingFormat = "GB18030"; //"GB2312";
        public const string SOFTWAREVERSION = "1.0";
        // Constant value for data recieve from Arduino
        public const uint TOTAL_FIELD_RECIEVE = 15;
        public const uint TEMPSTORAGE25 = 0;
        public const uint TEMP_DUALPIPE1 = 1;
        public const uint TEMP_DUALPIPE2 = 2;
        public const uint TEMP_BEFORECOOLER = 3;
        public const uint TURBIDITY = 4;
        public const uint CONDUCTIVITY = 5;
        public const uint LIGHT_INTENSITY = 6;
        public const uint MAIN_FLOW = 7;
        public const uint GAS_FLOW = 8;
        public const uint PIPE_FLOW1 = 9;
        public const uint PIPE_FLOW2 = 10;
        public const uint PIPE_FLOW3 = 11;
        public const uint LEVEL_NUTRIA = 12;
        public const uint LEVEL_STORAGE25 = 13;
        public const uint LEVEL_STERILE = 14;
        // Constant for save/restore user data from file config
        public const int COM_PORT = 0;
        public const int COM_BAURATE = 1;
        public const int STEPPER = 2;
        public const int MAIN_PUMP_SPEED = 3;


    }
}
