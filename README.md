# Date Taken

```
!!! NOTE: ALWAYS BACKUP YOUR FILES BEFORE DOING ANY OPERATIONS ON THEM. I am not responsible for any data losses due to the usage of this application. 
```

A C# Console application that sorts images by the date they were taken/created.

Sometimes, when you export from Major Cloud Providers, or have multiple sources of photos from devices, your directory can look like this:

```
top_level
├───Camera Roll
├───Google Backup
│   ├───takeout-20231011T134603Z-001
│   │   └───Takeout
│   │       └───Google Photos
│   │           ├───Photos from 2022
│   │           ├───Photos from 2023
│   │           ├───Taiwan Trip 
│   │           └───Trash
│   ├───takeout-20231011T134603Z-002
│   │   └───Takeout
│   │       └───Google Photos
│   │           ├───Photos from 2020
│   │           ├───Photos from 2021
├───Screenshots
├───Whatsapp
└───WhatsApp Images
```

When viewing photos, most of the time you are concerned with the moment where the memory occured. This tool will reorganise all the photos according to Day so your directory looks like this:

```
top_level
├───unsorted_items
├───files_with_issues
├───22-08-2023
├───14-08-2023
├───04-09-2023
├───13-08-2023
├───20-08-2023
```

The application uses the EXIF format information tied to your photo to attempt to sort the photo. If it cannot sort the photo, it goes into the `unsorted_items` folder. Any erroneous files or unsupported formats goes into the `files_with_issues` folder.

## Usage

Double Click the .exe file, and a Command prompt / Terminal will appear. Type in the FULL path of where you want to look for files. And where you would like to move your files. 

An example of how a FULL path looks like: 

`E:\Jordan Photos`

The application will search through for all the files in the directory you provided and ask for confirmation before moving on. 

If the file number does not look right, please press `Ctrl+C` to exit the application.

Upon confirmation, the application will start moving the files. This may take anywhere from a few minutes to a few hours, depending o nthe number of photos you have.

When done, the application will write "Done!"

## Technical Information

Dependencies: 

- .NET 7
- MetadataExtractor

To set up, just clone this directory, and use `dotnet run`, it should install all dependencies for you.
