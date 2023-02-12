/////////////////////Dictionary class - start///////////////////////////////
   function BVDictionary(){
      this.Count = 0;
      this.aKeys = new Array();
      this.aValues = new Array();
      this.Add = fBVDAdd;
      this.Item = fBVDItem;
      this.Keys = fBVDKeys;
   }

   function fBVDKeys(){
      return this.aKeys;
   }
   
   function fBVDAdd(lKey, lVal){
      if (this.Count != 0){
         for (var i=0;i<this.Count;i++){
            if (this.aKeys[i] == lKey){
               this.aValues[i] = lVal;
               return;
            }
         }
      }

      this.aKeys[this.Count] = lKey;
      this.aValues[this.Count] = lVal;
      this.Count++;
      return;
   }

   function fBVDItem(lKey){
      if (this.Count == 0) return "";
      for (var i=0;i<this.Count;i++){
         if (this.aKeys[i] == lKey){
            return this.aValues[i];
         }
      }
      
      return "";
   }

/////////////////Dictionary class - end///////////////////////////////
