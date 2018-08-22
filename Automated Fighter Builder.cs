
void Main(string argument)
{
  // block declarations
  string ERR_TXT = "";
  List<IMyTerminalBlock> l0 = new List<IMyTerminalBlock>();
  IMyPistonBase v0 = null;
  if(GridTerminalSystem.GetBlockGroupWithName("Projector Piston") != null) {
    GridTerminalSystem.GetBlockGroupWithName("Projector Piston").GetBlocksOfType<IMyPistonBase>(l0);
    if(l0.Count == 0) {
      ERR_TXT += "group Projector Piston has no Piston blocks\n";
    }
    else {
      for(int i = 0; i < l0.Count; i++) {
        if(l0[i].CustomName == "Piston") {
          v0 = (IMyPistonBase)l0[i];
          break;
        }
      }
      if(v0 == null) {
        ERR_TXT += "group Projector Piston has no Piston block named Piston\n";
      }
    }
  }
  else {
    ERR_TXT += "group Projector Piston not found\n";
  }
  List<IMyTerminalBlock> l1 = new List<IMyTerminalBlock>();
  IMyShipWelder v1 = null;
  if(GridTerminalSystem.GetBlockGroupWithName("WELDERS") != null) {
    GridTerminalSystem.GetBlockGroupWithName("WELDERS").GetBlocksOfType<IMyShipWelder>(l1);
    if(l1.Count == 0) {
      ERR_TXT += "group WELDERS has no Welder blocks\n";
    }
    else {
      for(int i = 0; i < l1.Count; i++) {
        if(l1[i].CustomName == "Welder") {
          v1 = (IMyShipWelder)l1[i];
          break;
        }
      }
      if(v1 == null) {
        ERR_TXT += "group WELDERS has no Welder block named Welder\n";
      }
    }
  }
  else {
    ERR_TXT += "group WELDERS not found\n";
  }
  List<IMyTerminalBlock> l2 = new List<IMyTerminalBlock>();
  IMyProjector v2 = null;
  if(GridTerminalSystem.GetBlockGroupWithName("PROJECTOR") != null) {
    GridTerminalSystem.GetBlockGroupWithName("PROJECTOR").GetBlocksOfType<IMyProjector>(l2);
    if(l2.Count == 0) {
      ERR_TXT += "group PROJECTOR has no Projector blocks\n";
    }
    else {
      for(int i = 0; i < l2.Count; i++) {
        if(l2[i].CustomName == "Projector") {
          v2 = (IMyProjector)l2[i];
          break;
        }
      }
      if(v2 == null) {
        ERR_TXT += "group PROJECTOR has no Projector block named Projector\n";
      }
    }
  }
  else {
    ERR_TXT += "group PROJECTOR not found\n";
  }
  
  // display errors
  if(ERR_TXT != "") {
    Echo("Script Errors:\n"+ERR_TXT+"(make sure block ownership is set correctly)");
    return;
  }
  else {Echo("");}
  


  // logic
  // v0 = piston
  // v1 = welder
  // v2 = projector
  // v3 = 


  printStep = 1;

// STEP 1: Retract piston to 0m, and turn off projector & Welders
  while(printStep >= 1){
      
     if(printStep == 1) {
         v0.SetValue("Velocity", (float)3);
         v0.ApplyAction("Retract");
         v1.ApplyAction("OnOff_Off");
         v2.ApplyAction("OnOff_Off");
    
        if(getExtraFieldFloat(v0, "Current position: (\\d+\\.?\\d*)") == 0) {
            printStep = 2;
        }}

// STEP 2: Extend piston to 10m SLOW, turn on projector & Welder
     if(printStep == 2) {
         v0.SetValue("Velocity", (float)0.5);
         v0.ApplyAction("Extend");
         v1.ApplyAction("OnOff_On");
         v2.ApplyAction("OnOff_On");

        if(getExtraFieldFloat(v0, "Current position: (\\d+\\.?\\d*)") == 10) {
            printStep = 3
        }}

// STEP 3: Stop piston, & turn off welders. Break any connection.
     if(printStep == 3) {
         v1.ApplyAction("OnOff_Off");
         v2.ApplyAction("OnOff_Off");

        if(getExtraFieldFloat(v0, "Current position: (\\d+\\.?\\d*)") == 10) {
            printStep = 0;
        }}
}

const string MULTIPLIERS = ".kMGTPEZY";

float getExtraFieldFloat(IMyTerminalBlock block, string regexString) {
  System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(regexString, System.Text.RegularExpressions.RegexOptions.Singleline);
  float result = 0.0f;
  double parsedDouble;
  System.Text.RegularExpressions.Match match = regex.Match(block.DetailedInfo);
  if (match.Success) {
    if (Double.TryParse(match.Groups[1].Value, out parsedDouble)) {
      result = (float) parsedDouble;
    }
    if(MULTIPLIERS.IndexOf(match.Groups[2].Value) > -1) {
      result = result * (float) Math.Pow(1000.0, MULTIPLIERS.IndexOf(match.Groups[2].Value));
    }
  }
  return result;
}

