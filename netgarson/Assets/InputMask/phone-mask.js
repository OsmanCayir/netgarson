class PhoneField {

    /***
        handler = the DOM object
        mask = any preferrable phone mask
        placeholder = character used to fill the space when char is deleted
        start = the position of the first num character user can enter
    ***/

    constructor(handler, mask = '+7(___)___-____', placeholder = '_') {
        this.handler = handler;
        this.mask = mask;
        this.placeholder = placeholder;

        //set the length
        this.setLength();

        //set value to placeholder
        this.setValue();

        //check where is the first enerable character index
        this.start = this.placeHolderPosition() - 1;

        //focused - move carette to the first placeholder
        this.handler.addEventListener('focusin', () => {
            this.focused();
        });

        //keydown - check key/remove placeholder/push numbers further or do nothing
        this.handler.addEventListener('keydown', (e) => {
            this.input(e);
        });
    }

    focused() {
        let placeholderPos = this.placeHolderPosition();
        this.handler.selectionStart = placeholderPos;
        this.handler.selectionEnd = placeholderPos;
    }

    input(e) {
        //unless it is a tab, prevent action
        if (!this.isDirectionKey(e.key)) {
            e.preventDefault();
        }

        //if integer, enter it
        if (this.isNum(e.key)) {
            this.changeChar(e.key);
        }
        //if user deletes, delete a number
        else if (this.isDeletionKey(e.key)) {

            if (e.key === 'Backspace') {
                let index = this.start;
                let dir = -1;
                this.changeChar(this.placeholder, dir, index);
            } else {
                this.changeChar(this.placeholder);
            }
        }
    }

    //put max length to the length of the mask
    setLength() {
        this.handler.maxLength = this.mask.length;
    }

    //set initial value
    setValue() {
        this.handler.value = this.mask;
    }

    //check if input is number
    isNum(i) {
        return !isNaN(i) && parseInt(Number(i)) == i && !isNaN(parseInt(i, 10));
    }

    //check if it is a button to delete stuff
    isDeletionKey(i) {
        return i === 'Delete' || i === 'Backspace';
    }

    //check if direction arrow
    isDirectionKey(i) {
        return i === 'ArrowUp' || i === 'ArrowDown' || i === 'ArrowRight' || i === 'ArrowLeft' || i === 'Tab';
    }

    //check if value is placeholder
    isPlaceholder(i) {
        return i == this.placeholder;
    }

    //check index of closest placeholder
    placeHolderPosition() {
        return this.handler.value.indexOf(this.placeholder);
    }

    changeChar(i, dir = 1, max = this.mask.length) {
        let val = this.handler.value;
        let pos;

        /**
         *  if direction is forward, character to be changed is before the caret
         *  else it is behind, so we move position one char back 
         */
        dir > 0 ? pos = this.handler.selectionStart : pos = this.handler.selectionStart - 1;

        let newVal = '';

        //if cursor at end, do nothing
        if (pos === max) {
            return false;
        }

        /**check if char to be replaced is placeholder or number    
        if it is placeholder, change it, if it is number
        push it, if it is neither, move cursor
        **/
        if (!this.isNum(val[pos]) && !this.isPlaceholder(val[pos])) {
            do {
                pos += dir;
                //if cursor at end, do nothing
                if (pos === max) {
                    return false;
                }

            } while (!this.isNum(val[pos]) && !this.isPlaceholder(val[pos]));
        }

        //replace char at index
        newVal = this.replaceAt(val, pos, i);

        //update the value in the field 
        this.handler.value = newVal;

        //move the caret if direction is forward
        if (dir > 0)
            pos += dir;

        this.handler.selectionStart = pos;
        this.handler.selectionEnd = pos;
    }

    replaceAt(str, pos, val) {
        return str.substring(0, pos) + val + str.substring(++pos);
    }
}

document.addEventListener('DOMContentLoaded', function () {

    "use strict";

    let field = document.getElementsByClassName('masked-phone');
    let phones = [];

    for (let x = 0; x < field.length; x++) {
        phones.push(new PhoneField(field[x], field[x].dataset.phonemask, field[x].dataset.placeholder));
    }

});