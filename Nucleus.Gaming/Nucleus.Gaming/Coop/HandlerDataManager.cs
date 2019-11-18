using Newtonsoft.Json;
using Nucleus.Gaming.Coop.Interop;
using Nucleus.Gaming.Package;
using System;
using System.IO;

namespace Nucleus.Gaming.Coop {
    public class HandlerDataManager : IDisposable {
        private GameHandlerMetadata handlerMetadata;
        private bool isDisposed;

        public HandlerData HandlerData { get; private set; }

        public HandlerDataEngine Engine { get; private set; }

#if WINFORMS
        public ContentManager Content { get; private set; }
#endif

        public HandlerDataManager(GameHandlerMetadata metadata, string jsCode) {
            Initialize(metadata, jsCode);
        }

        public HandlerDataManager(GameHandlerMetadata metadata, Stream stream) {
            using (StreamReader reader = new StreamReader(stream)) {
                Initialize(metadata, reader.ReadToEnd());
            }
        }

        private void Initialize(GameHandlerMetadata metadata, string jsCode) {
            this.handlerMetadata = metadata;

            Engine = new HandlerDataEngine(metadata, jsCode);

            string handlerStr = Engine.Initialize();
            HandlerData = JsonConvert.DeserializeObject<HandlerData>(handlerStr);

#if WINFORMS

            // content manager is shared within the same game
            Content = new ContentManager(metadata, HandlerData);
#endif
        }

        public void Dispose() {
            if (isDisposed) {
                return;
            }
            isDisposed = true;

            Engine.Dispose();

#if WINFORMS

            Content.Dispose();
#endif
        }

        public void Play(HandlerContext context, PlayerInfo player) {
#if WINFORMS

            // ugly solution
            context.PackageFolder = Content.PackageFolder;
            string contextData = Engine.Play(context, player);

            JsonConvert.PopulateObject(contextData, context);
#endif
        }
    }
}
