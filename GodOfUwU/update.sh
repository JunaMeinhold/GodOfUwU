#!/bin/bash
while [ -f ./lock ]; do sleep 1; done
sleep 1
cp ./tmp/* ./ -r
rm ./update.bat
rm ./update.sh
exec ./GodOfUwU