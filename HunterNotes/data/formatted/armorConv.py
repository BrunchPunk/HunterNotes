import os

setName = ""
armorName = ""
equipSlot = ""
armorFileName = ""
slot1Size = ""
slot2Size = ""
slot3Size = ""
skill1Name = ""
skill1Points = ""
skill2Name = ""
skill2Points = ""
skill3Name = ""
skill3Points = ""
skill4Name = ""
skill4Points = ""
ingredient1Name = ""
ingredient1Points = ""
ingredient2Name = ""
ingredient2Points = ""
ingredient3Name = ""
ingredient3Points = ""
ingredient4Name = ""
ingredient4Points = ""

def slotConv(inSlot):
    if('zmdi-minus' in inSlot):
        return '0'
    elif('zmdi-n-1-square' in inSlot):
        return '1'
    elif('zmdi-n-2-square' in inSlot):
        return '2'
    elif('zmdi-n-3-square' in inSlot):
        return '3'
    else:
        return '4'
#END slotConv def

def parseArmorFile(armorFileName):

    global setName
    global armorName
    global equipSlot
    global slot1Size
    global slot2Size
    global slot3Size
    global skill1Name
    global skill1Points
    global skill2Name
    global skill2Points
    global skill3Name
    global skill3Points
    global skill4Name
    global skill4Points
    global ingredient1Name
    global ingredient1Points
    global ingredient2Name
    global ingredient2Points
    global ingredient3Name
    global ingredient3Points
    global ingredient4Name
    global ingredient4Points

    innerParseMode = 1
    skillCount = 1
    ingredientCount = 1
    
    armorFile = open(armorFileName, 'r')

    for line in armorFile:
        if((innerParseMode == 1) and ('<div class="lead"><i class="zmdi' in line)): #slot 1 found
                slot1Size = slotConv(line)
                innerParseMode = 2
        elif(innerParseMode == 2): #get slot 2
            slot2Size = slotConv(line)
            innerParseMode = 3
        elif(innerParseMode == 3): # get slot 3
            slot3Size = slotConv(line)
            innerParseMode = 4
        elif((innerParseMode == 4) and ('<h4 class="card-header">Skills</h4>' in line)): #start searching for skills
            innerParseMode = 5
        elif((innerParseMode == 5) and ('<td><a href=' in line)): #skill name found
            tempSkillName = line[line.find('">') + 2:line.find('</a>')]

            if(skillCount == 1):
                skill1Name = '"' + tempSkillName + '"'
            elif(skillCount == 2):
                skill2Name = '"' + tempSkillName + '"'
            elif(skillCount == 3):
                skill3Name = '"' + tempSkillName + '"'
            elif(skillCount == 4):
                skill4Name = '"' + tempSkillName + '"'

            innerParseMode = 6
        elif(innerParseMode == 6): #get skill points
            tempSkillPoint = line[line.find('+') + 1:line.find('+') + 2]

            if(skillCount == 1):
                skill1Points = tempSkillPoint
            elif(skillCount == 2):
                skill2Points = tempSkillPoint
            elif(skillCount == 3):
                skill3Points = tempSkillPoint
            elif(skillCount == 4):
                skill4Points = tempSkillPoint
            
            skillCount = skillCount + 1
            innerParseMode = 5
        elif((innerParseMode == 5) and ('<h4 class="card-header">Crafting Materials</h4>' in line)): #end of skills start of ingredients
            innerParseMode = 7
        elif((innerParseMode == 7) and ('<td><a href=' in line)): #ingredient name found
            tempIngredientName = line[line.find('">') + 2:line.find('</a>')]

            if(ingredientCount == 1):
                ingredient1Name = '"' + tempIngredientName + '"'
            elif(ingredientCount == 2):
                ingredient2Name = '"' + tempIngredientName + '"'
            elif(ingredientCount == 3):
                ingredient3Name = '"' + tempIngredientName + '"'
            elif(ingredientCount == 4):
                ingredient4Name = '"' + tempIngredientName + '"'

            innerParseMode = 8
        elif(innerParseMode == 8): #get ingredient count
            tempIngredientCount = line[line.find('>x') + 2:line.find('>x') + 3]

            if(ingredientCount == 1):
                ingredient1Points = tempIngredientCount
            elif(ingredientCount == 2):
                ingredient2Points = tempIngredientCount
            elif(ingredientCount == 3):
                ingredient3Points = tempIngredientCount
            elif(ingredientCount == 4):
                ingredient4Points = tempIngredientCount
            
            ingredientCount = ingredientCount + 1
            innerParseMode = 7
        elif((innerParseMode == 7) and ('</table>' in line)): #that's it!
            break

    armorFile.close()
#END parseArmorFile def         

armorMainFile = open('armorRevised.txt', 'r', encoding='utf-8')
armorResultsFile = open('armorPart.csv', 'w', encoding='utf-8')
forgeResultsFile = open('forgePart.csv', 'w', encoding='utf-8')

parseMode = 1

for line in armorMainFile:
    if(line == "\n"): #RESET
        parseMode = 1

        setName = ""
        armorName = ""
        equipSlot = ""
        armorFileName = ""
        slot1Size = ""
        slot2Size = ""
        slot3Size = ""
        skill1Name = ""
        skill1Points = ""
        skill2Name = ""
        skill2Points = ""
        skill3Name = ""
        skill3Points = ""
        skill4Name = ""
        skill4Points = ""
        ingredient1Name = ""
        ingredient1Points = ""
        ingredient2Name = ""
        ingredient2Points = ""
        ingredient3Name = ""
        ingredient3Points = ""
        ingredient4Name = ""
        ingredient4Points = ""
    elif(parseMode == 1):
        setName = line.rstrip()
        print(setName)
        parseMode = 2
    elif(parseMode == 2):
        armorName = ""
        equipSlot = ""
        armorFileName = ""
        slot1Size = ""
        slot2Size = ""
        slot3Size = ""
        skill1Name = ""
        skill1Points = ""
        skill2Name = ""
        skill2Points = ""
        skill3Name = ""
        skill3Points = ""
        skill4Name = ""
        skill4Points = ""
        ingredient1Name = ""
        ingredient1Points = ""
        ingredient2Name = ""
        ingredient2Points = ""
        ingredient3Name = ""
        ingredient3Points = ""
        ingredient4Name = ""
        ingredient4Points = ""
        
        splitLine = line.split('|')
        
        armorName = '"' + splitLine[1] + '"'
        equipSlot = '"' + splitLine[0] + '"' 
        armorFileName = 'armor/' + splitLine[2].rstrip() + '.html'

        parseArmorFile(armorFileName) #reads necessary info into global variabls

        armorResultsFile.write(armorName + ',' +
                               equipSlot + ',' +
                               skill1Name + ',' +
                               skill1Points + ',' +
                               skill2Name + ',' +
                               skill2Points + ',' +
                               skill3Name + ',' +
                               skill3Points + ',' +
                               skill4Name + ',' +
                               skill4Points + ',' +
                               slot1Size + ',' +
                               slot2Size + ',' +
                               slot3Size + "\n")
        forgeResultsFile.write(armorName + ',' +
                               ingredient1Name + ',' +
                               ingredient1Points + ',' +
                               ingredient2Name + ',' +
                               ingredient2Points + ',' +
                               ingredient3Name + ',' +
                               ingredient3Points + ',' +
                               ingredient4Name + ',' +
                               ingredient4Points + "\n")
                               
                               
armorMainFile.close()
armorResultsFile.close()
forgeResultsFile.close()


