For reproduce
1. Run the app on .NET Core 5.0.100-preview.2.20176.6
2. Click "Create window" button
3. You will get the following exception 
    System.ArgumentException: Type must derive from Delegate. (Parameter 't') at at System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(IntPtr ptr, Type t)
    
If you run the app on .NET Core 3.1 or .NET Framework 4.6 or newer everything will be OK.