#!/usr/bin/ruby

root = $*[0]
project = $*[1]

#print("root: " + root)
#print("project: " +  project)

require 'rubygems'
require 'json'
require 'pp'
require 'yaml'
  
json = File.read(project + '/assets/game-manifest.json')
obj = JSON.parse(json)

changeVesion = false
version = ""
changeVersionCode = false
versionCode = ""

i = 0
while i < $*.size
    key = $*[i]
    if key.start_with? '-'
        key = key[1, key.size-1]
        value = ""
        if i+1 < $*.size
            if !$*[i+1].start_with? '-'
                value = $*[i+1]
            end
        end
        obj[key] = value

        if key == "version" 
            changeVesion = true
            version = value
        end

        if key == "version-code" or key == "build"
            changeVersionCode = true
            versionCode = value
        end
    end
    i = i + 1
end

pp "[apkremanifest] after:"
pp obj

json = obj.to_json
File.write(project + '/assets/game-manifest.json', json)

# 如果修改了版很号，还需要修改yaml文件
if changeVesion 
    yaml = File.read(project + '/apktool.yml')
    obj = YAML.load(yaml)
    if version.size == 1 
        version = "00" + version
    elsif version.size == 2
        version = "0" + version
    end
    ge = version[version.size-1, 1]
    shi = version[version.size-2, 1]
    bai = version[0, version.size-2]
    versionName = ""
    bai = "0" if bai == ""
    shi = "0" if shi == ""
    ge = "0" if ge == ""
    versionName = bai + "." + shi + "." + ge
    if obj['versionInfo'] == nil
        obj['versionInfo'] = {}
    end
    obj['versionInfo']['versionName'] = versionName

    yaml = YAML.dump(obj)
    File.write(project + '/apktool.yml', yaml)
end

if changeVersionCode
    yaml = File.read(project + '/apktool.yml')
    obj = YAML.load(yaml)
    obj['versionInfo']['versionCode'] = versionCode
    yaml = YAML.dump(obj)
    File.write(project + '/apktool.yml', yaml)
end