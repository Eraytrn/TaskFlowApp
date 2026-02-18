import { ref } from 'vue'

const toastState = ref({
    show: false,
    message: '',
    type: 'info' as 'success' | 'error' | 'info' | 'warning'
})

export function useToast() {
    function show(message: string, type: 'success' | 'error' | 'info' | 'warning' = 'info') {
        toastState.value = {
            show: true,
            message,
            type
        }

        setTimeout(() => {
            toastState.value.show = false
        }, 3000)
    }

    function close() {
        toastState.value.show = false
    }

    return {
        toast: toastState,
        show,
        close
    }
}
