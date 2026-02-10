#! /usr/bin/env bash
set -uvx
set -e
cd "$(dirname "$0")"
cwd=`pwd`
ts=`date "+%Y.%m%d.%H%M.%S"`
#dotnet run --project EasyObject.Demo/EasyObject.Demo.csproj --framework net462 "$@"
cd EasyObject.Demo
dotnet run --project EasyObject.Demo.csproj --framework net462 "$@"
