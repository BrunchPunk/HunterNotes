skillsFile = open('skills.txt', 'r')
resultsFile = open('skills.csv', 'w')

maxHolder = 1
skillHolder = ""

line = skillsFile.readline()

while line:
    if("rowspan" in line):
        maxHolder = int(line[9]) - 1
        skillHolder = skillsFile.readline()
        resultsFile.write('"' + skillHolder.rstrip() + '",')
        resultsFile.write(str(maxHolder) + ',"')
        line = skillsFile.readline()
    elif(line != "\n"):
        resultsFile.write(line.rstrip() + ' | ')
        line = skillsFile.readline()
    else:
        resultsFile.write('"' + "\n")
        line = skillsFile.readline()

resultsFile.write('"')

skillsFile.close()
resultsFile.close()
        
