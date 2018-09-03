decoFile = open('decorations.txt', 'r')
resultsFile = open('decorations.csv', 'w')

lineStyle = 1
decoName = ""
decoSize = ""
skillName = ""
owned = "0"

for line in decoFile:
    if(lineStyle == 1): #Deco name and size line
        decoName = '"' + line[:-2].rstrip() + '"'
        decoSize = line[-2:].rstrip()
        lineStyle = 2
    elif(lineStyle == 2): #Skill name
        skillName = '"' + line.rstrip() + '"'
        lineStyle = 3
    else: #Blank line
        resultsFile.write(decoName + ',' + skillName + ',' + decoSize + ',' + owned + "\n")
        lineStyle = 1


decoFile.close()
resultsFile.close()
