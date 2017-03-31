; extern FunctionEnter2Global:proc
; extern FunctionLeave2Global:proc
; extern FunctionTailcall2Global:proc


;typedef void FunctionEnter2(
;         rcx = FunctionID funcId, 
;         rdx = UINT_PTR clientData, 
;         r8  = COR_PRF_FRAME_INFO func, 
;         r9  = COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo);
_TEXT segment para 'CODE'

        align   16

        public  FunctionEnter2Naked

FunctionEnter2Naked    proc    frame

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

        ; call    FunctionEnter2Global

        add     rsp, 20h

        ; restore registers
        pop     r11
        pop     r10
        pop     rax

        ; return
        ret

FunctionEnter2Naked    endp

;typedef void FunctionLeave2(
;         rcx =  FunctionID funcId, 
;         rdx =  UINT_PTR clientData, 
;         r8  =  COR_PRF_FRAME_INFO func, 
;         r9  =  COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange);
_TEXT segment para 'CODE'

        align   16

        public  FunctionLeave2Naked

FunctionLeave2Naked    proc    frame

        ; save integer return register
        push    rax
        .allocstack 8

        sub     rsp, 20h
        .allocstack 20h

        .endprolog

        ; call    FunctionLeave2Global

        add     rsp, 20h

        ; restore integer return register
        pop                     rax

        ; return
        ret

FunctionLeave2Naked    endp

;typedef void FunctionTailcall2(
;         rcx =  FunctionID funcId, 
;         rdx =  UINT_PTR clientData, 
;         t8  =  COR_PRF_FRAME_INFO,

        align   16

        public  FunctionTailcall2Naked

FunctionTailcall2Naked   proc    frame

        ; save rax
        push    rax
        .allocstack 8

        sub     rsp, 20h
        .allocstack 20h

        .endprolog

        ; call    FunctionTailcall2Global

        add     rsp, 20h

        ; restore rax
        pop     rax

        ; return
        ret

FunctionTailcall2Naked   endp

_TEXT ends

end