using System;
using System.Globalization;
using System.IO.Pipes;

public class SamplesRegionInfo  {

   public static void Main1()  {

      // Creates a RegionInfo using the ISO 3166 two-letter code.
      RegionInfo myRI1 = new RegionInfo( "US" );

      // Creates a RegionInfo using a CultureInfo.LCID.
      RegionInfo myRI2 = new RegionInfo( new CultureInfo("en-US",false).LCID );

      using (AnonymousPipeServerStream pipeServer =
                  new AnonymousPipeServerStream(PipeDirection.Out,
                  HandleInheritability.Inheritable)){
      using(StreamWriter sw = new StreamWriter(pipeServer)){
         //{fact rule=correctness-regioninfo-interop@v1.0 defects=1}
         //ruleid: correctness-regioninfo-interop
         sw.WriteLine(myRI1);
         //{/fact}

         //{fact rule=correctness-regioninfo-interop@v1.0 defects=0}
         //ok
         sw.WriteLine(myRI2);
      }}
      //{/fact}
   }
}