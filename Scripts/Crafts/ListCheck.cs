using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Check
{
    public class ListCheck : MonoBehaviour
{

    public static List<Block> DestroyFromList(Block block, List<Block> blocks) {
        List<Block> copy_blocks = blocks.ToList();
        for (int i =0; i<copy_blocks.Count; i++) {
            if (copy_blocks[i].name == block.name) {
                copy_blocks.RemoveAt(i);
            } 
        }
        return copy_blocks;
    }
    public static List<int> CountItems(List<Block> ToGetCount) {
        List<int> Copy = new List<int>() {};
        foreach (Block val in ToGetCount.Distinct())
        {
           Copy.Add(ToGetCount.Where(x => x == val).Count());
        }       
        return Copy;
    }

    public static bool BlockInList(Block item, List<Block> container){
        string name = item.name;
            for (int j = 0; j < container.Count; j++){
                if(name == container[j].name){
                    return true;
                }
            }
            return false;
        }

    public static bool ListContainsNew(List<Block> content, List<Block> container){

        List<Block> ContainerCopy = container.ToList();
        List<Block> ContentCopy = content.ToList();

        for(int i = ContentCopy.Count -1 ; i >= 0; i--){

            if(BlockInList(ContentCopy[i], ContainerCopy)){
                ContainerCopy.Remove(ContentCopy[i]);
                ContentCopy.Remove(ContentCopy[i]);
                }
            }
        return ContentCopy.Count == 0;
        }
    }
}



