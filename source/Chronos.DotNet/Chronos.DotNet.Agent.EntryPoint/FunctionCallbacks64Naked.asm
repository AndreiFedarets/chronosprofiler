extern FunctionEnterGlobal:proc
extern FunctionLeaveGlobal:proc
extern FunctionTailcallGlobal:proc

_TEXT segment para 'CODE'

;========================================================================================================

;typedef void FunctionEnter3Naked(
;         rcx = FunctionIDOrClientID functionIDOrClientID);

        align   16

        public  FunctionEnter3Naked

FunctionEnter3Naked     proc    frame

        ; save registers
        push    rax
        .allocstack 8
        push    r10
        .allocstack 8
        push    r11
        .allocstack 8

        sub     rsp, 20h
        .allocstack 20h

        .endprolog

        call    FunctionEnterGlobal

        add     rsp, 20h

        ; restore registers
        pop     r11
        pop     r10
        pop     rax

        ; return
        ret

FunctionEnter3Naked     endp

;========================================================================================================

;typedef void FunctionLeave3Naked(
;         rcx = FunctionIDOrClientID functionIDOrClientID);

        align   16

        public  FunctionLeave3Naked

FunctionLeave3Naked     proc    frame

        ; save integer return register
        push    rax
        .allocstack 8

        sub     rsp, 20h
        .allocstack 20h

        .endprolog

        call    FunctionLeaveGlobal

        add     rsp, 20h

        ; restore integer return register
        pop     rax

        ; return
        ret

FunctionLeave3Naked     endp

;========================================================================================================

;typedef void FunctionTailcall3Naked(
;         rcx = FunctionIDOrClientID functionIDOrClientID);

        align   16

        public  FunctionTailcall3Naked

FunctionTailcall3Naked  proc    frame

        ; save rax
        push    rax
        .allocstack 8

        sub     rsp, 20h
        .allocstack 20h

        .endprolog

        call    FunctionTailcallGlobal

        add     rsp, 20h

        ; restore rax
        pop     rax

        ; return
        ret

FunctionTailcall3Naked  endp

_TEXT ends

end