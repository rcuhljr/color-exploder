using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using AssemblyCSharp;

public static class IOUtils
{

  private static Stage Load (string fileName)
  {
    try {
      string line;
      StreamReader theReader = new StreamReader (fileName, Encoding.Default);

      using (theReader) {
        do {
          line = theReader.ReadLine ();
          
          if (line != null) {
            string[] entries = line.Split (',');
            if (entries.Length > 0){
            }
//              DoStuff (entries);
          }
        } while (line != null);
        
        // Done reading, close the reader and return true to broadcast success
        theReader.Close ();
        return new Stage(null);
      }
    }
    catch (Exception e) {
      Debug.Log(e.Message);
      return null;
    }
  }
}
