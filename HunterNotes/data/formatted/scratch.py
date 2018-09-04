armorFile = open("armor.txt", "r")
outFile = open("armorRevised.txt", "w")

setName = ""
fileName = ""
armorName = ""
curSlot = "head"

parseMode = 1

for line in armorFile:
    if(parseMode == 1):
        if(not (line == "\n")): 
            setName = line.rstrip()
            outFile.write(setName + "\n")
        else:
            parseMode = 2
    elif(parseMode == 2):
        if(line == "\n"):
            parseMode = 3
        elif(len(line) == 2):
            next
        else:
            fileName = line[(line.rfind("/") + 1):line.rfind('"')]
            armorName = line[(line.rfind(">") + 1):-1]
            outFile.write(curSlot + "|" + armorName + "|" + fileName + "\n")
    elif(parseMode == 3):            
        if("<a href" in line):
            if(curSlot == "head"):
                curSlot = "chest"
            elif(curSlot == "chest"):
                curSlot = "arms"
            elif(curSlot == "arms"):
                curSlot = "waist"
            elif(curSlot == "waist"):
                curSlot = "legs"
            fileName = line[(line.rfind("/") + 1):line.rfind('"')]
            armorName = line[(line.rfind(">") + 1):-1]
            outFile.write(curSlot + "|" + armorName + "|" + fileName + "\n")
            parseMode = 2
        else:
            parseMode = 4
    elif(parseMode == 4):
        parseMode = 5
    elif(parseMode == 5):
        if(line == "\n"):
            parseMode = 1
            curSlot = "head"
            outFile.write("\n")
        else:
            parseMode = 4


armorFile.close()
outFile.close()
