﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRentals.Contract
{
    public static class Context
    {
        [ThreadStatic]
        static Action<string> _explanations;

        static Action<string> _globalTrace;

        public static void Debug(string format, params object[] args)
        {
            if (_globalTrace != null)
            {
                _globalTrace(string.Format(format, args));
                return;
            }
            Trace.WriteLine(string.Format(format, args));
        }

        public static void Explain(string format, params object[] args)
        {
            if (null != _explanations)
            {
                _explanations(string.Format(format, args));
                return;
            }


            if (_globalTrace != null)
            {
                _globalTrace(string.Format(format, args));
                return;
            }
            Trace.WriteLine(string.Format(format, args));
        }

        public static ExplainCapture CaptureForThread(Action<string> optionalLogger = null)
        {
            var log = new StringBuilder();

            Action<string> builder = s => log.AppendLine(s);
            if (optionalLogger != null)
            {
                builder += optionalLogger;
            }
            var previous = Interlocked.Exchange(ref _explanations, builder);

            return new ExplainCapture(() =>
            {
                Interlocked.CompareExchange(ref _explanations, builder, previous);
                if (log.Length > 0)
                {
                    previous(log.ToString().TrimEnd('\r', '\n'));
                }
            }, log);
        }


        sealed class Disposable : IDisposable
        {
            readonly Action _action;

            public Disposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }

        public sealed class ExplainCapture : IDisposable
        {
            readonly StringBuilder _builder;

            readonly Action _action;

            public ExplainCapture(Action action, StringBuilder builder)
            {
                _action = action;
                _builder = builder;
            }

            public string Log
            {
                get { return _builder.ToString().TrimEnd('\r', '\n'); }
            }

            public void Dispose()
            {
                _action();
            }
        }

        public static void SwapForDebug(Action<string> builder = null)
        {
            _globalTrace = builder;
        }
    }
}
