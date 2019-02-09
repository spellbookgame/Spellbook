﻿using Bolt;
using UnityEngine;
using UnityEngine.UI;
/*
    Written by Moises Martinez moi.compsci@gmail.com
     */
namespace Photon.Lobby
{
    public class SpellCasterLobbyChoose : Bolt.EntityEventListener<ILobbySpellcasterSelectState>
    {
        // Bolt
        public BoltConnection connection;

        // Lobby
        // By default, no body has selected anything yet.
        public bool alchemistChosen = false;
        public bool arcanistChosen = false;
        public bool elementalistChosen = false;
        public bool chronomancerChosen = false;
        public bool illusionistChosen = false;
        public bool summonerChosen = false;

        public Button alchemistButton;
        public Button arcanistButton;
        public Button elementalistButton;
        public Button chronomancerButton;
        public Button illusionistButton;  //aka tricksterButton
        public Button summonerButton;

        public Button readyButton;
        public Text text;

        public Color alchemistColor = Color.green;
        public Color arcanistColor = Color.magenta;
        public Color elementalistColor = Color.red;
        public Color chronomancerColor = Color.yellow; //will change to brown later.
        public Color illusionistColor = Color.cyan;
        public Color summonerColor = Color.blue;

        public Color unselectedColor = Color.grey; // = Color.white;

        // Keep track of what the local player chooses.
        int previousSelected = -1;
        int currentSelected = -1;

        //public static LobbyPhotonPlayer localPlayer;  //May need to use this instead of "Player" below.
        Player localPlayer;
        
        // Handlers
        public override void Attached()
        {
            //Innefficient, for demoing purposes.
            alchemistButton = GameObject.Find("button_alchemist").GetComponent<Button>();
            arcanistButton = GameObject.Find("button_arcanist").GetComponent<Button>();
            elementalistButton = GameObject.Find("button_elementalist").GetComponent<Button>(); 
            chronomancerButton = GameObject.Find("button_chronomancer").GetComponent<Button>();
            illusionistButton = GameObject.Find("button_trickster").GetComponent<Button>();
            summonerButton = GameObject.Find("button_summoner").GetComponent<Button>();
            text = GameObject.Find("ChooseClass").GetComponent<Text>();
            // A callback is basically another way of saying "getting an update from the network"
            state.AddCallback("AlchemistSelected", () =>
            {
                if (state.AlchemistSelected)
                {
                    alchemistButton.image.color = alchemistColor;
                    alchemistButton.interactable = false;
                }
                else
                {
                    unselectedColor.a = 1;
                    alchemistButton.image.color = unselectedColor;
                    alchemistButton.interactable = true;
                }
            });

            state.AddCallback("ArcanistSelected", () =>
            {
                if (state.ArcanistSelected)
                {
                    arcanistButton.image.color = arcanistColor;
                    arcanistButton.interactable = false;
                }
                else
                {
                    unselectedColor.a = 1;
                    arcanistButton.image.color = unselectedColor;
                    arcanistButton.interactable = true;
                }
            });

            state.AddCallback("ElementalistSelected", () =>
            {
                if (state.ElementalistSelected)
                {
                    elementalistButton.image.color = elementalistColor;
                    elementalistButton.interactable = false;
                }
                else
                {
                    unselectedColor.a = 1;
                    elementalistButton.image.color = unselectedColor;
                    elementalistButton.interactable = true;
                }
            });

            state.AddCallback("ChronomancerSelected", () =>
            {
                if (state.ChronomancerSelected)
                {
                    chronomancerButton.image.color = chronomancerColor;
                    chronomancerButton.interactable = false;
                }
                else
                {
                    unselectedColor.a = 1;
                    chronomancerButton.image.color = unselectedColor;
                    chronomancerButton.interactable = true;
                }
            });

            state.AddCallback("IllusionistSelected", () =>
            {
                if (state.IllusionistSelected)
                {
                    illusionistButton.image.color = illusionistColor;
                    illusionistButton.interactable = false;
                }
                else
                {
                    unselectedColor.a = 1;
                    illusionistButton.image.color = unselectedColor;
                    illusionistButton.interactable = true;
                }
            });

            state.AddCallback("SummonerSelected", () =>
            {
                if (state.SummonerSelected)
                {
                    summonerButton.image.color = summonerColor;
                    summonerButton.interactable = false;
                }
                else
                {
                    unselectedColor.a = 1;
                    summonerButton.image.color = unselectedColor;
                    summonerButton.interactable = true;
                }
            });
        }

        public override void ControlGained()
        {
            BoltConsole.Write("ControlGained", Color.blue);
            SetupPlayer();
        }

        public override void SimulateController()
        {
            ISpellcasterSelectCommandInput input = SpellcasterSelectCommand.Create();
            input.alchemistChosen = alchemistChosen;
            input.arcanistChosen = arcanistChosen;
            input.elementalistChosen = elementalistChosen;
            input.chronomancerChosen = chronomancerChosen;
            input.illusionistChosen = illusionistChosen;
            input.summonerChosen = summonerChosen;
            entity.QueueInput(input);
        }

        public override void ExecuteCommand(Command command, bool resetState)
        {
            // May have to delete this after testing.
            if (!entity.isOwner) { return; }

            if (!resetState && command.IsFirstExecution)
            {
                SpellcasterSelectCommand selectCommand = command as SpellcasterSelectCommand;
                state.AlchemistSelected = selectCommand.Input.alchemistChosen;
                state.ArcanistSelected = selectCommand.Input.arcanistChosen;
                state.ElementalistSelected = selectCommand.Input.elementalistChosen;
                state.ChronomancerSelected = selectCommand.Input.chronomancerChosen;
                state.IllusionistSelected = selectCommand.Input.illusionistChosen;
                state.SummonerSelected = selectCommand.Input.summonerChosen;
            }
        }

        public void SetupPlayer()
        {
            BoltConsole.Write("SetupPlayer", Color.green);
            Debug.Log("setup player");
            //localPlayer = this;

            
            this.transform.SetParent(GameObject.Find("LobbyPanel").transform);

            //Hardcoded for prototyping and testing
            this.transform.localPosition = new Vector3(200f, -800f, 0f);
            this.transform.localScale = new Vector3(.6f, .6f, 1f);
            
            

            localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();

            alchemistButton.onClick.RemoveAllListeners();
            alchemistButton.onClick.AddListener(OnAlchemistClicked);

            arcanistButton.onClick.RemoveAllListeners();
            arcanistButton.onClick.AddListener(OnArcanistClicked);

            elementalistButton.onClick.RemoveAllListeners();
            elementalistButton.onClick.AddListener(OnElementalistClicked);

            chronomancerButton.onClick.RemoveAllListeners();
            chronomancerButton.onClick.AddListener(OnChronomancerClicked);

            illusionistButton.onClick.RemoveAllListeners();
            illusionistButton.onClick.AddListener(OnIllusionistClicked);

            summonerButton.onClick.RemoveAllListeners();
            summonerButton.onClick.AddListener(OnSummonerClicked);

        }

        // UI
        public void OnAlchemistClicked()
        {
            previousSelected = currentSelected;
            currentSelected = 0;
            alchemistChosen = true;
            openPreviousSpellcaster();
            localPlayer.onClickAclhemist();
            text.text = "You chose Alchemist!";
        }

        public void OnArcanistClicked()
        {
            previousSelected = currentSelected;
            currentSelected = 1;
            arcanistChosen = true;
            openPreviousSpellcaster();
            localPlayer.onClickArcanist();
            text.text = "You chose Arcanist!";
        }

        public void OnElementalistClicked()
        {
            previousSelected = currentSelected;
            currentSelected = 2;
            elementalistChosen = true;
            openPreviousSpellcaster();
            localPlayer.onClickElementalist();
            text.text = "You chose Elementalist!";
        }

        public void OnChronomancerClicked()
        {
            previousSelected = currentSelected;
            currentSelected = 3;
            chronomancerChosen = true;
            openPreviousSpellcaster();
            localPlayer.onClickChronomancer();
            text.text = "You chose Chronomancer!";
        }

        public void OnIllusionistClicked()
        {
            previousSelected = currentSelected;
            currentSelected = 4;
            illusionistChosen = true;
            openPreviousSpellcaster();
            localPlayer.onClickTrickster();
            text.text = "You chose Illusionist!";
        }

        public void OnSummonerClicked()
        {
            previousSelected = currentSelected;
            currentSelected = 5;
            summonerChosen = true;
            openPreviousSpellcaster();
            localPlayer.onClickSummoner();
            text.text = "You chose Summoner!";

        }

        void openPreviousSpellcaster()
        {
            switch (previousSelected)
            {
                case 0:
                    alchemistChosen = false;
                    break;
                case 1:
                    arcanistChosen = false;
                    break;
                case 2:
                    elementalistChosen = false;
                    break;
                case 3:
                    chronomancerChosen = false;
                    break;
                case 4:
                    illusionistChosen = false;
                    break;
                case 5:
                    summonerChosen = false;
                    break;
            }
        }

    }

}