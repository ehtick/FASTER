﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FASTER.core;

namespace FASTER.server
{
    internal struct ServerKVFunctions<Key, Value, Input, Output, Functions> : IFunctions<Key, Value, Input, Output, long>
        where Functions : IFunctions<Key, Value, Input, Output, long>
    {
        private readonly Functions functions;
        private readonly FasterKVServerSessionBase<Output> serverNetworkSession;

        public ServerKVFunctions(Functions functions, FasterKVServerSessionBase<Output> serverNetworkSession)
        {
            this.functions = functions;
            this.serverNetworkSession = serverNetworkSession;
        }

        public void CheckpointCompletionCallback(int sessionID, string sessionName, CommitPoint commitPoint)
            => functions.CheckpointCompletionCallback(sessionID, sessionName, commitPoint);

        public void SingleDeleter(ref Key key, ref Value value, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address)
            => functions.SingleDeleter(ref key, ref value, ref recordInfo, ref updateInfo, address);

        public void PostSingleDeleter(ref Key key, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address) { }

        public bool ConcurrentDeleter(ref Key key, ref Value value, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address)
            => functions.ConcurrentDeleter(ref key, ref value, ref recordInfo, ref updateInfo, address);

        public bool ConcurrentReader(ref Key key, ref Input input, ref Value value, ref Output dst, ref RecordInfo recordInfo, long address)
            => functions.ConcurrentReader(ref key, ref input, ref value, ref dst, ref recordInfo, address);

        public bool ConcurrentWriter(ref Key key, ref Input input, ref Value src, ref Value dst, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address)
            => functions.ConcurrentWriter(ref key, ref input, ref src, ref dst, ref output, ref recordInfo, ref updateInfo, address);

        public bool NeedInitialUpdate(ref Key key, ref Input input, ref Output output, ref UpdateInfo updateInfo)
            => functions.NeedInitialUpdate(ref key, ref input, ref output, ref updateInfo);

        public bool NeedCopyUpdate(ref Key key, ref Input input, ref Value oldValue, ref Output output, ref UpdateInfo updateInfo)
            => functions.NeedCopyUpdate(ref key, ref input, ref oldValue, ref output, ref updateInfo);

        public bool CopyUpdater(ref Key key, ref Input input, ref Value oldValue, ref Value newValue, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address)
            => functions.CopyUpdater(ref key, ref input, ref oldValue, ref newValue, ref output, ref recordInfo, ref updateInfo, address);

        public bool PostCopyUpdater(ref Key key, ref Input input, ref Value oldValue, ref Value newValue, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address)
            => functions.PostCopyUpdater(ref key, ref input, ref oldValue, ref newValue, ref output, ref recordInfo, ref updateInfo, address);

        public bool InitialUpdater(ref Key key, ref Input input, ref Value value, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address)
            => functions.InitialUpdater(ref key, ref input, ref value, ref output, ref recordInfo, ref updateInfo, address);

        public bool InPlaceUpdater(ref Key key, ref Input input, ref Value value, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address)
            => functions.InPlaceUpdater(ref key, ref input, ref value, ref output, ref recordInfo, ref updateInfo, address);

        public void PostInitialUpdater(ref Key key, ref Input input, ref Value value, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address) { }

        public void ReadCompletionCallback(ref Key key, ref Input input, ref Output output, long ctx, Status status, RecordMetadata recordMetadata)
        {
            serverNetworkSession.CompleteRead(ref output, ctx, status);
            functions.ReadCompletionCallback(ref key, ref input, ref output, ctx, status, recordMetadata);
        }

        public void RMWCompletionCallback(ref Key key, ref Input input, ref Output output, long ctx, Status status, RecordMetadata recordMetadata)
        {
            serverNetworkSession.CompleteRMW(ref output, ctx, status);
            functions.RMWCompletionCallback(ref key, ref input, ref output, ctx, status, recordMetadata);
        }

        public bool SingleReader(ref Key key, ref Input input, ref Value value, ref Output dst, ref RecordInfo recordInfo, long address)
            => functions.SingleReader(ref key, ref input, ref value, ref dst, ref recordInfo, address);

        public bool SingleWriter(ref Key key, ref Input input, ref Value src, ref Value dst, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address, WriteReason reason)
            => functions.SingleWriter(ref key, ref input, ref src, ref dst, ref output, ref recordInfo, ref updateInfo, address, reason);

        public void PostSingleWriter(ref Key key, ref Input input, ref Value src, ref Value dst, ref Output output, ref RecordInfo recordInfo, ref UpdateInfo updateInfo, long address, WriteReason reason) { }

        public void DisposeKey(ref Key key) { functions.DisposeKey(ref key); }

        public void DisposeValue(ref Value value) { functions.DisposeValue(ref value); }
    }
}
