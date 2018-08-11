#!/usr/bin/ruby

# 加载 XcodeModify 魔改版本
file_dir = File.dirname(__FILE__) 

# 装载参数
android_studio_project_path=$*[0]

# 参数守卫
if android_studio_project_path.nil?
    puts "invalid android_studio_project_path"
    exit 1 
end

puts ""
puts "[load_ruby_code.rb]"
puts "android_studio_project_path: " + android_studio_project_path 

require "rexml/document"  
include REXML  
puts "-- Unity Setting --"  

# 修改 AndroidManifest 中的 package，因为改模块是作为库，不可与应用最终 package 相同
AndroidManifestPath = android_studio_project_path + "/game-unity/src/main/AndroidManifest.xml"
manifest = Document.new(File.open(AndroidManifestPath))
package = manifest.root.attributes["package"]
versionCode = manifest.root.attributes["android:versionCode"]
versionName = manifest.root.attributes["android:versionName"]
puts "package: " + package
puts "versionCode: " + versionCode
puts "versionName: " + versionName
manifest.root.attributes["package"]=package+".game_unity"
xmlString=manifest.to_s
File.write(AndroidManifestPath, xmlString)