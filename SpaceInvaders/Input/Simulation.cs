using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class Simulation
    {
        public enum State
        {
            Realtime,
            FixedStep,
            SingleStep,
            Pause
        };

        // singleton access
        private static Simulation PrivGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);
            return pInstance;
        }

        public static void Create()
        {
            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new Simulation();
            }
        }

        public static void Update(float systemTime)
        {
            Simulation pSim = Simulation.PrivGetInstance();
            Debug.Assert(pSim != null);

            // update input
            pSim.PrivProcessInput();

            // Time update.
            //      Get the time that has passed.
            //      Feels backwards, but its not, need to see how much time has passed
            pSim.stopWatch_toc = systemTime - pSim.stopWatch_tic;
            pSim.stopWatch_tic = systemTime;

            if (pSim.PrivGetState() == State.FixedStep)
            {
                pSim.timeStep = SIM_SINGLE_TIME_STEP;
            }
            else if (pSim.PrivGetState() == State.Realtime)
            {
                pSim.timeStep = pSim.stopWatch_toc;
            }
            else if (pSim.PrivGetState() == State.SingleStep)
            {
                pSim.timeStep = SIM_SINGLE_TIME_STEP;
                pSim.PrivSetState(State.Pause);
            }
            else //  pSim->getState() == SimulationEnum::Pause
            {
                pSim.timeStep = 0.0f;
            }

            pSim.totalWatch += pSim.timeStep;

        }


        // --- Simulation controls ------------
        //   S - single step
        //   D - repeat step while holding
        //   G - start simulation fixed step
        //   H - start simulation realtime stepping
        private void PrivProcessInput()
        {
            // ------------------------------------------------------------------
            //  SIMULATION Controls 
            // ------------------------------------------------------------------

            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_G) == true)
            {
                this.PrivSetState(State.FixedStep);
            }
            else if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_H) == true)
            {
                this.PrivSetState(State.Realtime);
            }

            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_S) && (oldKey == false))
            {
                // Do only once "a single step"
                this.PrivSetState(State.SingleStep);
            }

            if (Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_D) == true)
            {
                // repeating "a single step"
                this.PrivSetState(State.SingleStep);
            }


            oldKey = Azul.Input.GetKeyState(Azul.AZUL_KEY.KEY_S);

        }

        // Get / Set state

        private void PrivSetState(State simState)
        {
            this.state = simState;
        }
        private State PrivGetState()
        {
            return this.state;
        }
        public static void SetState(State simState)
        {
            Simulation pSim = Simulation.PrivGetInstance();
            Debug.Assert(pSim != null);

            pSim.PrivSetState(simState);
        }
        public static State GetState()
        {
            Simulation pSim = Simulation.PrivGetInstance();
            Debug.Assert(pSim != null);

            return pSim.PrivGetState();
        }
        public static float GetTimeStep()
        {
            Simulation pSim = Simulation.PrivGetInstance();
            Debug.Assert(pSim != null);
            return pSim.timeStep;
        }
        public static float GetTotalTime()
        {
            Simulation pSim = Simulation.PrivGetInstance();
            Debug.Assert(pSim != null);
            return pSim.totalWatch;
        }

        // cannot create without going through singleton
        private Simulation()
        {
            this.state = State.Realtime;

            this.timeStep = 0.0f;
            this.totalWatch = 0.0f;
            this.stopWatch_tic = 0.0f;
            this.stopWatch_toc = 0.0f;
        }

        // ----------------------------------------------
        // data:
        // ----------------------------------------------

        private static Simulation pInstance;

        private State state;

        private float stopWatch_tic;
        private float stopWatch_toc;
        private float totalWatch;
        private float timeStep;

        private const int SIM_NUM_WAKE_CYCLES = 0;
        private const float SIM_SINGLE_TIME_STEP = 0.016666f;

        private static bool oldKey = false;
    }
}

// End of File
