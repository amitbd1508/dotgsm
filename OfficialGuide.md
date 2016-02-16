# Source files #

After you check out the whole source codes, you will find out there are two sub projects under the DotGSM solution folder: a .Net assembly project “DotGSM” and a ASP.NET web project. DotGSM is the main project for this library and ASP.NET web project is a sample project for demonstrating purpose.

DotGSM sub project contains five source files writing in C sharp language which are Coordinates.cs, DotGSMSrcBuilder.cs, MapType.cs, MarkerColor.cs and MarkerSign.cs. DotGSMSrcBuilder.cs is the core class and it contains all the main program logic.

# How to use DotGSM library #

To use DotGSM by checking out latest source code, simply add “DotGSM.dll” reference into your project from "Check out folder\DotGSM\bin\Release" and using “Circube.DotGSM” namespace in your source code. Or you can download the latest compiled assembly and reference it from your application directly.

# Geocoding #

You can call a static method of DotGSMBuilder class named “Geocoding” for extracting latitude and longitude information by giving the address string:

Coordinates coordinates = DotGSMBuilder.Geocoding("Taipei", [Google API Key ](.md));

It takes two parameters: address string at first argument and Google API Key at second argument. The return value is an object instance of Coordinates class. The Coordinates class contains two public member valuables which represent latitude and longitude in double format type. you can check the sample at the Default.aspx.cs souse file under sub ASP.NET web project in DotGSM source code. A “localhost” Google API Key is used for demo. you should apply for a unique Google API Key to your own web domain to make sure that your code work fine with Google online services. For the detailed explanation of Google Geocoding, please check the document from http://code.google.com/apis/maps/documentation/services.html#Geocoding.

# Generate source path of Google Static Maps #

It is the main purpose of this library. From the sample code below, you will find out that all operations relate to the object instance of DotGSMBuilder class:

DotGSMBuilder dotgsm = new DotGSMBuilder([Google API Key ](.md));

dotgsm.SetCenter(new Coordinates(25.08000, 121.5600));

dotgsm.SetMapSize(350, 350);

dotgsm.SetZoomLevel(10);

dotgsm.SetMapType(MapType.roadmap);

dotgsm.AutoAdjust = false;

dotgsm.AddMarker(new Coordinates(25.08532, 121.5615), MarkerColor.blue, MarkerSign.c);

string image\_res = dotgsm.Src;

form1.Controls.Add(dotgsm.BuildHtmlImage());

The first step is to create an object instance of DotGSMBuilder class and pass Google API Key as argument. It is really a simple job as showing. Then, by calling the member method of DotGSMBuilder class you can set center, map size, zoom level, may type into the map. On official guide of Google Static Maps, the center and zoom level are optional parameters if you have provided marker information. The DotGSMBuilder keeps the same behavior but it provides an additional control option “AutoAdjust” to let developer decides if program logic should generate center and zoom level information automatically when you have added any marker on the map or not. By assigning "true" to "AutoAdjust" property the DotGSMBuilder will tell Google Static Maps to generate center and zoom level information based on the marker information. However, if you don't provide any marker information, DotGSMBuilder will use default values even you set "AutoAdjust" to true.

For the default value to each property, please reference from below:

center: 0,0 (latitude, longitude)

map size: 300, 300 (width, height)

zoom level: 12

map type: roadmap

auto adjust: true

Finally, when all the setting have been done, developer could simply retrieve "Src" property of DotGSMBuilder and assign it to the src property of HTML image tag. Moreover, directly call BuildHtmlImage method to get a ASP.NET HTML image control.