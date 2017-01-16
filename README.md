HTML to PDF (Pechkin fork) for .NET 4.6.1 and Azure Service Fabric
=======

.NET Wrapper for [WkHtmlToPdf](http://github.com/antialize/wkhtmltopdf) DLL, library that uses Webkit engine to convert HTML pages to PDF.

This is fork that upgrades and adds support for .NET 4.6.1, removes all the logging and ensures support for running PDF generation on Azure Service Fabric cluster applications.

Upgrades to use latest version of xunit, and compile only as x64 which is the only supported target platform for Azure Service Fabric.

This version of the WkHtmlToPdf wrapper, removes support for external resources such as JS, CSS and images. You have to embed the style and images (base64 encoded) within the content of the HTML.

FAQ
---

### Q: Why produced PDF lacks background images and colors? ###

**A:** By default, all backgrounds will be ommited from the document.

You can override this setting by calling `SetPrintBackground(true)` on the `ObjectConfig` supplied with the HTML document to the `Convert()` method of the converter.

### Q: Can I use this for regular ASP.NET project and not Azure Service Fabric?

**A:** The library have only been tested and developed for use with an Azure Service Fabric service. If you want a library to generate HTML to PDF using ASP.NET, check out the [TuesPechkin](https://github.com/tuespetre/TuesPechkin) library.

### Q: Why can't I have links to external resources, such as images?

**A:** This possibility have been removed, it relies on additional DLL files from OpenSSL, which are not included. You could look at the original Pechkin library and include those if needed.

To embed images to PDF, base64 encode and put them into the HTML document like this: Example: src="data:image/jpeg;base64,/9j/4AAQSkZJRgABAgAAAQABA

### Q: PDF generation does not work, Exception Info: System.DllNotFoundException

**A:** You need to manually install the C++ redist (64-bit) for PDF generation to work: [https://www.microsoft.com/en-us/download/details.aspx?id=53587](https://www.microsoft.com/en-us/download/details.aspx?id=53587)


NuGet
-----

PechinSF is not yet available on NuGet. Download the source and compile using Visual Studio 2015.

Usage
-----

To include Pechkin in your service, add a reference to **Pechkin.dll** and **Pechkin.Synchronized.dll**, then add the **wkhtmltox.dll** either as full file in root of your service project, or as a link. Then set the
"Copy to Output Directory" to "Copy if newer". After you compile your project, verify of all those 3 .dll files are copied to your bin folder.

You need to manually install the Microsoft Visual C++ 2015 Redistributable (64-bit) on your cluster virtual machines for pinvoke to work.

Pechkin is both easy to use

```csharp
byte[] pdfBuf = new SimplePechkin(new GlobalConfig()).Convert("<html><body><h1>Hello world!</h1></body></html>");
```

and functional

```csharp
// create global configuration object
GlobalConfig gc = new GlobalConfig();

// set it up using fluent notation
gc.SetMargins(new Margins(300, 100, 150, 100))
  .SetDocumentTitle("Test document")
  .SetPaperSize(PaperKind.Letter);
//... etc

// create converter
IPechkin pechkin = new SynchronizedPechkin(gc);

// subscribe to events
pechkin.Begin += OnBegin;
pechkin.Error += OnError;
pechkin.Warning += OnWarning;
pechkin.PhaseChanged += OnPhase;
pechkin.ProgressChanged += OnProgress;
pechkin.Finished += OnFinished;

// create document configuration object
ObjectConfig oc = new ObjectConfig();

// and set it up using fluent notation too
oc.SetCreateExternalLinks(false)
  .SetFallbackEncoding(Encoding.ASCII)
  .SetLoadImages(false)
  .SetPageUri("http://google.com");
//... etc

// convert document
byte[] pdfBuf = pechkin.Convert(oc);
```

License
-------

Copyright 2012 by Slava Kolobaev. This work is made available under the terms of the Creative Commons Attribution 3.0 license, http://creativecommons.org/licenses/by/3.0/

Copyright 2016 by Sondre Bjellås. This work is made available under the terms of the Creative Commons Attribution 3.0 license, http://creativecommons.org/licenses/by/3.0/
