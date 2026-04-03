#! /usr/bin/env bash
# -*- mode: sh -*-
script_dir="$(realpath `dirname "$0"`)"
cd "$script_dir"
#set -uvx
set -e
cwd=$(pwd)
ts=$(date "+%Y.%m%d.%H%M.%S")
ver=$(echo $ts | sed -e "s/[.]0/./g")
source ~/.emacs.d/snippets/sh-mode/color-defs.inc.sh

green script_dir=$script_dir
green cwd=$cwd
green ts=$ts
green ver=$ver

# 特定のテスト（Test902など）だけを、詳細ログ付きで回す
dotnet test --filter Name~Test902 --logger "console;verbosity=detailed"
