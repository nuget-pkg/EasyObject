#! /usr/bin/env bash
# -*- mode: sh -*-
chcp.com 65001 > /dev/null 2>&1
script_dir="$(dirname "$0")"
script_dir="$(realpath $script_dir)"
str="$1"
if [[ "${str:0:1}" == "@" ]]; then
  if [[ "$str" == "@run" ]]; then
    shift
  elif [[ "$str" == "@exe" ]]; then
    if [ ! -f "$script_dir/demo01.exe" ]; then
      "$script_dir/.r.demo01.sh" "@merge" -f 1>&2
    fi
    shift
    $script_dir/demo01.exe "$@"
  else
    "$script_dir/.r.demo01.sh" "$@"
  fi
else
  cscs -nuget:restore "$script_dir/demo01.main.cs" 1>&2
  cscs -l:0 "$script_dir/demo01.main.cs" "$@"
fi
