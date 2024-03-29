# Visual Studio Project Zip Application

Use this application to create a zip archive of a Visual Studio project
without directories like `bin/` and `obj/` or `.vs/` or `.git/` and
without `.gitignore` or `.gitattributes` files.

## Features

* command-line arguments for adjusting the settings
* settings include:
    * output directory
    * output name
    * directories to skip (both as additional directories and 
      as directories replacing the default setting)
    * files to skip (both as additional files and
      as files replacing the default setting)

## Roadmap

* settings like additional directories or files to skip (Done)
* ability to change desired location and filename for the output archive (Done)
* Unit tests for `VSProjectZip.Core` project (Done)

## Libraries used in the project

- Moq (Mocking library)
- NUnit (Unit testing library)

## How to use

To use the application you need to pass command-line arguments.

### Syntax

```shell
VSProjectZip.exe <DIRECTORY_TO_ZIP>
[--outdir=<OUTPUT_DIRECTORY>]
[--outname=<OUTPUT_NAME>]
[--override-skipfiles]
[--override-skipdirs]
[--skipfiles=<FILES_TO_SKIP>]
[--skipdirs=<DIRECTORIES_TO_SKIP>]
```

### Arguments

* `<DIRECTORY_TO_ZIP>`

    The directory that needs to be zipped.

* `--outdir=<OUTPUT_DIRECTORY>`

    Specifies the output directory where the zip archive will be created in.
    If not specified, it will default to `DIRECTORY_TO_ZIP`'s parent directory.

* `--outname=<OUTPUT_NAME>`
	
    Specifies the exact filename that the archive will get.
    If not specified, it will default to `<DIRECTORY_TO_ZIP>`'s name with `.zip` extension.

    For example, users can set the output archive's name to `Project.zip` by passing `--outname="Project.zip"`.

* `--skipfiles=<FILES_TO_SKIP>`

    Additional files separated by `|` (pipe symbol) that need to be skipped when zipping the specified directory.
    For example, users can request skipping the `.dockerignore` and `logs.txt` files by passing `--skipfiles=".dockerignore | logs.txt"` (whitespaces are not mandatory).

    * `--override-skipfiles`

        If specified, the `--skipfiles=<FILES_TO_SKIP>` option will use only the specified `<FILES_TO_SKIP>`
        instead of using them along with the default set of files.

* `--skipdirs=<DIRECTORIES_TO_SKIP>`

    Additional directories separated by `|` (pipe symbol) that need to be skipped when zipping the specified directory.
    For example, users can request skipping the `packages` and `publish` directories by passing `--skipdirs="packages | publish"` (whitespaces are not mandatory).

    * `--override-skipdirs`

        If specified, the `--skipdirs=<DIRECTORIES_TO_SKIP>` option will use only the specified `<DIRECTORIES_TO_SKIP>`
        instead of using them along with the default set of directories.
