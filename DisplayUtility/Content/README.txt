1. Make sure MonoGame 3.8 is installed
2. Launch Monogame Pipeline Editor by doubleclicking Content.mgcb in Solution
3. Edit as needed (add new graphics, fonts, etc)
4. Rebuild in Pipeline Editor.
5. New .xnb files appear in .\Content\bin\Windows\ (these are assets/images converted to MonoGame format)
6. Visual Studio post-build batch will copy built .xnb to the .\Content folder where .exe is

If having problems, debug by making sure the files exist in both locations; 
