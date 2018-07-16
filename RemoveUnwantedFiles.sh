#!/bin/sh
#Remove All BIN+OBJ Folders to force remove any old binary object references
find . -iname "bin" -type d | xargs rm -rf
find . -iname "obj" -type d | xargs rm -rf
find . -iname "Logs" -type d | xargs rm -rf
