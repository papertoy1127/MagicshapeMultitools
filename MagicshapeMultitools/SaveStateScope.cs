using System;

namespace MagicshapeMultitools {
    public class SaveStateScope : IDisposable {
        public SaveStateScope(scnEditor editor, bool clearRedo = false, bool onlySelectionChanged = false,
            bool skipSaving = false) {
            this.editor = editor;
            if (!skipSaving) {
                editor.SaveState(clearRedo, onlySelectionChanged);
            }

            editor.changingState++;
        }

        public void Dispose() {
            this.editor.changingState--;
        }

        private scnEditor editor;
    }
}