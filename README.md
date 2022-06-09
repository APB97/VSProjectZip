# Visual Studio Project Zip Application

Use this application to create a zip archive of a Visual Studio project without directories like `bin/` and `obj/` or `.vs/` or `.git/`.

## Features

* command-line arguments for adjusting the settings
* settings include:
	* output directory
	* directories to skip (both as additional directories and as directories replacing the default setting)
	* files to skip (both as additional files and as files replacing the default setting)

## Roadmap

* settings like additional directories or files to skip (Done)
* ability to change desired location and filename for the output archive (Users can only change desired location for now)

## How to use (Work in Progress)

To use the application you need to pass command-line arguments.

### Syntax

```shell
VSProjectZip.exe <DIRECTORY_TO_ZIP>
[--outdir=<OUTPUT_DIRECTORY>]
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

* `--skipfiles=<FILES_TO_SKIP>`

	Additional files separated by `|` (pipe symbol) that need to be skipped when zipping the specified directory. For example, users can request skipping the `.dockerignore` and `logs.txt` files by passing `--skipfiles=".dockerignore | logs.txt"` (whitesspaces are not mandatory).

	* `--override-skipfiles`

		If specified, the `--skipfiles=<FILES_TO_SKIP>` option will use only the specified `<FILES_TO_SKIP>` instead of using them along with the default set of files.

* `--skipdirs=<DIRECTORIES_TO_SKIP>`

	Additional directories separated by `|` (pipe symbol) that need to be skipped when zipping the specified directory. For example, users can request skipping the `packages` and `publish` directories by passing `--skipdirs="packages | directories"` (whitesspaces are not mandatory).

	* `--override-skipdirs`

		If specified, the `--skipdirs=<DIRECTORIES_TO_SKIP>` option will use only the specified `<DIRECTORIES_TO_SKIP>` instead of using them along with the default set of directories.
