using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_MONITORING
{
    class DataProcessing
    {
        public Int32 MainPumpSpeed;
        public Int32 StepperSpeed;
        public enum Position
        {
            NUTRIA_LEVEL,
            STORAGE25_TEMP,
            STORAGE25_LEVEL,
            STORAGE25_PH,
            STORAGE25_PPM,
            STERILE_LEVEL,
            DUAL_PIPE_TEMP,
            DUAL_PIPE_TEMPU
        }

        public int MainProcessing(string[] _strInput)
        {

            Int32 nutriA = Convert.ToInt32(_strInput[(uint)Position.NUTRIA_LEVEL]);
            Int32 sterile = Convert.ToInt32(_strInput[(uint)Position.STORAGE25_TEMP]);

            nutriA += 9;
            sterile += 7;
            if((nutriA * sterile) > 100)
            {
                MainPumpSpeed = 50;
                StepperSpeed = 90;
            }
            else
            {
                MainPumpSpeed = 40;
                StepperSpeed = 80;
            }
            return nutriA * sterile;
        }
    }
}
