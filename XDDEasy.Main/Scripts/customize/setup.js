; (function($, window, document, undefined) {
    var win = $(window),
        doc = $(document),
        bod = $(document.body),
        autoWatch = true,
        watching = true,
        setupFunctions = [],
        clearFunctions = [],
        resizeInt = false,
        mediaQueries = [[10, 'mobile-portrait'], [20, 'mobile-landscape'], [30, 'tablet-portrait'], [40, 'tablet-landscape'], [50, 'desktop']],
        hiresTestHeight = 20,
        fixedTest,
        supportFixed = true,
        fixed = $(),
        touchMoved = false,
        touchId = 0,
        init = false;
    $.template = {
        keys: {
            tab: 9,
            enter: 13,
            space: 32,
            left: 37,
            up: 38,
            right: 39,
            down: 40
        },
        respondPath: $('script').last().attr('src').replace(/[^\/]+$/, '') + 'libs/',
        mediaQuery: {
            name: 'mobile-portrait',
            on: ['mobile-portrait'],
            hires: false,
            has: function(name) {
                return ($.inArray(name, $.template.mediaQuery.on) > -1);
            },
            is: function(name) {
                return ($.template.mediaQuery.name.indexOf(name.toLowerCase()) === 0);
            },
            isSmallerThan: function(name) {
                return ! $.template.mediaQuery.has(name);
            }
        },
        ie7: !!(document.all && !document.querySelector),
        ie8: !!(document.all && document.querySelector && !document.getElementsByClassName),
        iPhone: !!navigator.userAgent.match(/iPhone/i),
        iPod: !!navigator.userAgent.match(/iPod/i),
        iPad: !!navigator.userAgent.match(/iPad/i),
        android: !!navigator.userAgent.match(/Android/i)
    };
    $.template.iOs = ($.template.iPhone || $.template.iPod || $.template.iPad);
    $.template.touchOs = ($.template.iOs || $.template.android);
    $.template.viewportWidth = win.width();
    $.template.viewportHeight = $.template.iPhone ? window.innerHeight: win.height();
    yepnope({
        test: Modernizr.mq('(min-width:0)'),
        nope: [$.template.respondPath + 'respond.min.js']
    });
    if (Modernizr.touch) {
        doc.on('touchstart',
            function(event) {
                touchMoved = false; ++touchId;
            }).on('touchmove',
            function(event) {
                touchMoved = true;
            });
    }
    $.template.processTouchClick = function(target, event) {
        if (!event) {
            return true;
        }
        if (event.type === 'touchend') {
            if (!touchMoved) {
                $(target).data('touchstart-ID', touchId);
                return true;
            }
            else {
                return false;
            }
        }
        else if (event.type === 'click') {
            if ($(target).data('touchstart-ID') === touchId) {
                return false;
            }
            else {
                return true;
            }
        }
        return true;
    };
    fixedTest = $('<div style="position:fixed; top:0"></div>').appendTo(bod);
    function _checkPositionFixed() {
        var top = fixedTest.offset().top,
            scroll = doc.scrollTop();
        if (scroll < 2) {
            return;
        }
        if (Math.round(top) != Math.round(scroll)) {
            supportFixed = false;
            fixed.css({
                right: 'auto',
                bottom: 'auto'
            });
            var scrollFixed = function() {
                _setPositionFixed(fixed);
            };
            scrollFixed();
            doc.on('scroll', scrollFixed);
            win.on('normalized-resize orientationchange', scrollFixed);
        }
        fixedTest.remove();
        doc.off('scroll', _checkPositionFixed);
    }
    doc.on('scroll', _checkPositionFixed);
    function _setPositionFixed(elements) {
        var scrollTop = doc.scrollTop(),
            scrollLeft = doc.scrollLeft(),
            width = $.template.viewportWidth,
            height = $.template.viewportHeight;
        elements.each(function(i) {
            var element = $(this),
                positions = element.data('fixed-position'),
                offsetTop,
                offsetLeft;
            if (positions.top) {
                offsetTop = positions.top.percentage ? positions.top.value * height: positions.top.value;
                element.css('top', (scrollTop + offsetTop) + 'px');
            }
            if (positions.left) {
                offsetLeft = positions.left.percentage ? positions.left.value * width: positions.left.value;
                element.css('left', (scrollLeft + offsetLeft) + 'px');
            }
            if (positions.right) {
                if (positions.left) {
                    element.width(width - offsetLeft - (positions.right.percentage ? positions.right.value * width: positions.right.value));
                }
                else {
                    element.css('left', (scrollLeft + (positions.right.percentage ? (1 - positions.right.value) * width: width - positions.right.value) - element.outerWidth()) + 'px');
                }
            }
            if (positions.bottom) {
                if (positions.top) {
                    element.height(height - offsetTop - (positions.bottom.percentage ? positions.bottom.value * height: positions.bottom.value));
                }
                else {
                    element.css('top', (scrollTop + (positions.bottom.percentage ? (1 - positions.bottom.value) * height: height - positions.bottom.value) - element.outerHeight()) + 'px');
                }
            }
        });
    }
    $.fn.detectFixedBounds = function() {
        this.css({
            top: '',
            right: '',
            bottom: '',
            left: '',
            width: '',
            height: ''
        });
        this.each(function() {
            var element = $(this),
                sides = ['top', 'right', 'bottom', 'left'],
                positions = {},
                i,
                value;
            for (i = 0; i < sides.length; ++i) {
                value = element.css(sides[i]);
                if (value.match(/^-?[0-9]+px$/)) {
                    positions[sides[i]] = {
                        value: parseInt(value, 10),
                        percentage: false
                    };
                }
                else if (value.match(/^-?[0-9]+px$/)) {
                    positions[sides[i]] = {
                        value: parseFloat(value) / 100,
                        percentage: true
                    };
                }
            }
            if (!positions.top && !positions.bottom) {
                positions.top = {
                    value: 0,
                    percentage: false
                };
            }
            if (!positions.left && !positions.right) {
                positions.left = {
                    value: 0,
                    percentage: false
                };
            }
            element.data('fixed-position', positions);
        });
        if (!supportFixed) {
            this.css({
                right: 'auto',
                bottom: 'auto'
            });
            _setPositionFixed(this);
        }
        return this;
    };
    $.fn.enableFixedFallback = function() {
        fixed = fixed.add(this.detectFixedBounds());
    };
    $.fn.disableFixedFallback = function() {
        this.css({
            top: '',
            right: '',
            bottom: '',
            left: '',
            width: '',
            height: ''
        });
        fixed = fixed.not(this.removeData('fixed-position'));
    };
    $.fn.parseCSSValue = function(prop, def) {
        var parsed = parseInt(this.css(prop), 10);
        return isNaN(parsed) ? (def || 0) : parsed;
    };
    $.fn.hasInlineCSS = function(prop) {
        if (this.length === 0) {
            return false;
        }
        var regex = new RegExp('(^| |\t|;)' + prop + '\s*:', 'i');
        return regex.test(this.getStyleString());
    };
    $.fn.getStyleString = function() {
        if (this.length === 0) {
            return '';
        }
        var string = !$.support.style ? this[0].style.cssText.toLowerCase() : this[0].getAttribute('style');
        return (string || '');
    };
    $.fn.filterFollowing = function(selector, fromLast) {
        var selection = $(),
            next;
        if (!selector || selector === '') {
            return selection.add(this);
        }
        else if (this.length === 0) {
            return selection;
        }
        next = this[fromLast ? 'last': 'first']();
        while (next.is(selector)) {
            selection = selection.add(next);
            next = next[fromLast ? 'prev': 'next']();
        }
        return selection;
    };
    $.fn.prevImmediates = function(selector) {
        return this.prevAll().filterFollowing(selector);
    };
    $.fn.nextImmediates = function(selector) {
        return this.nextAll().filterFollowing(selector);
    };
    $.fn.childrenImmediates = function(selector, fromLast) {
        return this.children().filterFollowing(selector, fromLast);
    };
    $.fn.tempShow = function() {
        var affected = $();
        this.each(function(i) {
            var element = $(this);
            if (element.css('display') === 'none') {
                affected = affected.add(element.show());
            }
            element.parentsUntil('body').each(function() {
                var parent = $(this),
                    added = false;
                if (parent.css('display') === 'none') {
                    affected = affected.add(parent.show());
                    added = true;
                }
                if (this.nodeName.toLowerCase() === 'details' && !this.open) {
                    parent.prop('open', true).data('tempShowDetails', true);
                    if (!added) {
                        affected = affected.add(parent);
                    }
                }
                previous = parent;
            });
        });
        return affected;
    };
    $.fn.tempShowRevert = function() {
        return this.css('display', '').each(function(i) {
            var element = $(this);
            if (element.css('display') !== 'none' && !element.data('tempShowDetails')) {
                element.css('display', 'none');
            }
            if (this.nodeName.toLowerCase() === 'details' && element.data('tempShowDetails')) {
                element.prop('open', false).removeData('tempShowDetails');
            }
        });
    };
    var sizeWatcher = {
        sizeElements: $(),
        widthElements: $(),
        heightElements: $(),
        scrollElements: $(),
        interval: 250,
        timeout: false,
        watch: function() {
            if ($.isReady) {
                sizeWatcher.sizeElements.each(function(i) {
                    var element = $(this),
                        width = element.width(),
                        height = element.height(),
                        data = element.data('sizecache') || {
                            width: 0,
                            height: 0
                        };
                    if (width != data.width || height != data.height) {
                        element.data('sizecache', {
                            width: width,
                            height: height
                        });
                        element.trigger('sizechange', [width != data.width, height != data.height]);
                    }
                });
                sizeWatcher.widthElements.each(function(i) {
                    var element = $(this),
                        width = element.width(),
                        data = element.data('widthcache') || 0;
                    if (width != data) {
                        element.data('widthcache', width);
                        element.trigger('widthchange', [width]);
                    }
                });
                sizeWatcher.heightElements.each(function(i) {
                    var element = $(this),
                        height = element.height(),
                        data = element.data('heightcache') || 0;
                    if (height != data) {
                        element.data('heightcache', height);
                        element.trigger('heightchange', [height]);
                    }
                });
                sizeWatcher.scrollElements.each(function(i) {
                    var element = $(this),
                        width = this.scrollWidth,
                        height = this.scrollHeight,
                        data = element.data('scrollcache') || {
                            width: 0,
                            height: 0
                        };
                    if (width != data.width || height != data.height) {
                        element.data('scrollcache', {
                            width: width,
                            height: height
                        });
                        element.trigger('scrollsizechange', [width != data.width, height != data.height]);
                    }
                });
            }
            sizeWatcher.timeout = setTimeout(sizeWatcher.watch, sizeWatcher.interval);
        },
        start: function() {
            if (!sizeWatcher.timeout) {
                sizeWatcher.timeout = setTimeout(sizeWatcher.watch, sizeWatcher.interval);
            }
        },
        stop: function() {
            if (sizeWatcher.sizeElements.length === 0 && sizeWatcher.widthElements.length === 0 && sizeWatcher.heightElements.length === 0 && sizeWatcher.scrollElements.length === 0) {
                clearTimeout(sizeWatcher.timeout);
                sizeWatcher.timeout = false;
            }
        }
    };
    $.event.special.sizechange = {
        setup: function() {
            var element = $(this);
            element.data('sizecache', {
                width: element.width(),
                height: element.height()
            });
            sizeWatcher.sizeElements = sizeWatcher.sizeElements.add(this);
            sizeWatcher.start();
        },
        teardown: function() {
            sizeWatcher.sizeElements = sizeWatcher.sizeElements.not(this);
            $(this).removeData('sizecache');
            sizeWatcher.stop();
        }
    };
    $.event.special.widthchange = {
        setup: function() {
            var element = $(this);
            element.data('widthcache', element.width());
            sizeWatcher.widthElements = sizeWatcher.widthElements.add(this);
            sizeWatcher.start();
        },
        teardown: function() {
            sizeWatcher.widthElements = sizeWatcher.widthElements.not(this);
            $(this).removeData('widthcache');
            sizeWatcher.stop();
        }
    };
    $.event.special.heightchange = {
        setup: function() {
            var element = $(this);
            element.data('heightcache', element.height());
            sizeWatcher.heightElements = sizeWatcher.heightElements.add(this);
            sizeWatcher.start();
        },
        teardown: function() {
            sizeWatcher.heightElements = sizeWatcher.heightElements.not(this);
            $(this).removeData('heightcache');
            sizeWatcher.stop();
        }
    };
    $.event.special.scrollsizechange = {
        setup: function() {
            $(this).data('scrollcache', {
                width: this.scrollWidth,
                height: this.scrollHeight
            });
            sizeWatcher.scrollElements = sizeWatcher.scrollElements.add(this);
            sizeWatcher.start();
        },
        teardown: function() {
            sizeWatcher.scrollElements = sizeWatcher.scrollElements.not(this);
            $(this).removeData('scrollcache');
            sizeWatcher.stop();
        }
    };
    $.fn.sizechange = function(fn) {
        return (typeof fn === 'function') ? this.on('sizechange', fn) : this.trigger('sizechange');
    };
    $.fn.widthchange = function(fn) {
        return (typeof fn === 'function') ? this.on('widthchange', fn) : this.trigger('widthchange');
    };
    $.fn.heightchange = function(fn) {
        return (typeof fn === 'function') ? this.on('heightchange', fn) : this.trigger('heightchange');
    };
    $.fn.scrollsizechange = function(fn) {
        return (typeof fn === 'function') ? this.on('scrollsizechange', fn) : this.trigger('scrollsizechange');
    };
    $.each([{
        name: 'wrapAll',
        clear: false,
        setup: {
            prepare: false,
            target: function() {
                return this.parent();
            },
            self: true,
            subs: false
        }
    },
        {
            name: 'wrapInner',
            clear: false,
            setup: {
                prepare: false,
                target: function() {
                    return this.children();
                },
                self: true,
                subs: false
            }
        },
        {
            name: 'wrap',
            clear: false,
            setup: {
                prepare: false,
                target: function() {
                    return this.parent();
                },
                self: true,
                subs: false
            }
        },
        {
            name: 'unwrap',
            clear: {
                target: function() {
                    return this.parent();
                },
                self: true,
                subs: false
            },
            setup: false
        },
        {
            name: 'append',
            clear: false,
            setup: {
                prepare: function() {
                    return this.children();
                },
                target: function(prepared) {
                    return this.children().not(prepared);
                },
                self: true,
                subs: true
            }
        },
        {
            name: 'prepend',
            clear: false,
            setup: {
                prepare: function() {
                    return this.children();
                },
                target: function(prepared) {
                    return this.children().not(prepared);
                },
                self: true,
                subs: true
            }
        },
        {
            name: 'before',
            clear: false,
            setup: {
                prepare: function() {
                    return this.prevAll();
                },
                target: function(prepared) {
                    return this.prevAll().not(prepared);
                },
                self: true,
                subs: true
            }
        },
        {
            name: 'after',
            clear: false,
            setup: {
                prepare: function() {
                    return this.nextAll();
                },
                target: function(prepared) {
                    return this.nextAll().not(prepared);
                },
                self: true,
                subs: true
            }
        },
        {
            name: 'remove',
            clear: {
                target: function() {
                    return this;
                },
                self: true,
                subs: true
            },
            setup: false
        },
        {
            name: 'empty',
            clear: {
                target: function() {
                    return this;
                },
                self: false,
                subs: true
            },
            setup: false
        },
        {
            name: 'html',
            clear: {
                target: function() {
                    return this;
                },
                self: false,
                subs: true
            },
            setup: {
                prepare: false,
                target: function() {
                    return this;
                },
                self: true,
                subs: false
            }
        }],
        function() {
            var func = this,
                original = $.fn[func.name];
            $.fn[func.name] = function() {
                var target,
                    prepared = false,
                    result;
                if (autoWatch && watching) {
                    if (func.clear) {
                        func.clear.target.call(this).applyClear(func.clear.self, func.clear.sub);
                    }
                    if (func.setup && func.setup.prepare) {
                        prepared = func.setup.prepare.call(this);
                    }
                }
                watching = false;
                result = original.apply(this, Array.prototype.slice.call(arguments));
                watching = true;
                if (autoWatch && watching && func.setup) {
                    func.setup.target.call(this, prepared).applySetup(func.setup.self, func.setup.sub);
                }
                return result;
            };
        });
    $.template.enableDOMWatch = function() {
        autoWatch = true;
    };
    $.template.disableDOMWatch = function() {
        var previous = autoWatch;
        autoWatch = false;
        return previous;
    };
    $.template.addClearFunction = function(func, priority) {
        clearFunctions[priority ? 'unshift': 'push'](func);
    };
    $.fn.addClearFunction = function(func, priority) {
        this.each(function(i) {
            var element = $(this),
                functions = element.data('clearFunctions') || [];
            if (!functions.length || $.inArray(func, functions) < 0) {
                functions[priority ? 'unshift': 'push'](func);
                element.addClass('withClearFunctions').data('clearFunctions', functions);
            }
        });
        return this;
    };
    $.fn.removeClearFunction = function(func) {
        this.each(function() {
            var element = $(this),
                functions = element.data('clearFunctions') || [],
                i;
            for (i = 0; i < functions.length; ++i) {
                if (functions[i] === func) {
                    functions.splice(i, 1); --i;
                }
            }
            if (functions.length > 0) {
                element.data('clearFunctions', functions);
            }
            else {
                element.removeClass('withClearFunctions').removeData('clearFunctions');
            }
        });
        return this;
    };
    $.fn.applyClear = function(self, children) {
        var element = this,
            isWatching = $.template.disableDOMWatch();
        if (self === undefined) self = true;
        if (children === undefined) children = true;
        $.each(clearFunctions,
            function() {
                element = this.call(element, self, children);
            });
        if (isWatching) {
            $.template.enableDOMWatch();
        }
        return this;
    };
    $.template.addSetupFunction = function(func, priority) {
        setupFunctions[priority ? 'unshift': 'push'](func);
    };
    $.fn.applySetup = function(self, children) {
        var element = this,
            isWatching = $.template.disableDOMWatch();
        if (self === undefined) self = true;
        if (children === undefined) children = true;
        $.each(setupFunctions,
            function() {
                this.call(element, self, children);
            });
        if (isWatching) {
            $.template.enableDOMWatch();
        }
        return this;
    };
    $.fn.findIn = function(self, children, selector) {
        var element = $(this);
        if (self && children) {
            return element.filter(selector).add(element.find(selector));
        }
        else {
            return element[self ? 'filter': 'find'](selector);
        }
    };
    $.template.addSetupFunction(function(self, children) {
        if ($.fn.details) {
            this.findIn(self, children, 'details').details();
        }
        if ($('html').hasClass('no-generatedcontent')) {
            var iconMap = {
                'plus': '&#61462;',
                'minus': '&#61465;',
                'info': '&#61470;',
                'left-thin': '&#61581;',
                'up-thin': '&#61583;',
                'right-thin': '&#61582;',
                'down-thin': '&#61580;',
                'level-up': '&#61588;',
                'level-down': '&#61587;',
                'switch': '&#61591;',
                'infinity': '&#61635;',
                'squared-plus': '&#61464;',
                'squared-minus': '&#61467;',
                'home': '&#61474;',
                'keyboard': '&#61499;',
                'erase': '&#61636;',
                'pause': '&#61594;',
                'forward': '&#61598;',
                'backward': '&#61599;',
                'next': '&#61596;',
                'previous': '&#61597;',
                'hourglass': '&#61540;',
                'stop': '&#61593;',
                'triangle-up': '&#61575;',
                'play': '&#61592;',
                'triangle-right': '&#61574;',
                'triangle-down': '&#61572;',
                'triangle-left': '&#61573;',
                'adjust': '&#61544;',
                'cloud': '&#61619;',
                'star': '&#61448;',
                'star-empty': '&#61449;',
                'cup': '&#61511;',
                'list': '&#61457;',
                'moon': '&#61622;',
                'heart-empty': '&#61447;',
                'heart': '&#61446;',
                'music-note': '&#61440;',
                'beamed-note': '&#61441;',
                'thumbs': '&#61456;',
                'flag': '&#61483;',
                'tools': '&#61527;',
                'gear': '&#61526;',
                'warning': '&#61503;',
                'lightning': '&#61621;',
                'record': '&#61595;',
                'thunder-cloud': '&#61620;',
                'voicemail': '&#61641;',
                'plane': '&#61623;',
                'mail': '&#61445;',
                'pencil': '&#61495;',
                'feather': '&#61496;',
                'tick': '&#61458;',
                'cross': '&#61459;',
                'cross-round': '&#61460;',
                'squared-cross': '&#61461;',
                'question': '&#61468;',
                'quote': '&#61492;',
                'plus-round': '&#61463;',
                'minus-round': '&#61466;',
                'right': '&#61570;',
                'arrow': '&#61509;',
                'fwd': '&#61491;',
                'undo': '&#61584;',
                'redo': '&#61585;',
                'left': '&#61569;',
                'up': '&#61571;',
                'down': '&#61568;',
                'list-add': '&#61607;',
                'numbered-list': '&#61606;',
                'left-fat': '&#61577;',
                'right-fat': '&#61578;',
                'up-fat': '&#61579;',
                'down-fat': '&#61576;',
                'add-user': '&#61452;',
                'question-round': '&#61469;',
                'info-round': '&#61471;',
                'eye': '&#61479;',
                'price-tag': '&#61480;',
                'cloud-upload': '&#61488;',
                'reply': '&#61489;',
                'reply-all': '&#61490;',
                'code': '&#61493;',
                'extract': '&#61494;',
                'printer': '&#61497;',
                'refresh': '&#61498;',
                'speech': '&#61500;',
                'chat': '&#61501;',
                'card': '&#61505;',
                'directions': '&#61506;',
                'marker': '&#61507;',
                'map': '&#61508;',
                'compass': '&#61510;',
                'trash': '&#61512;',
                'page': '&#61513;',
                'page-list-inverted': '&#61517;',
                'pages': '&#61514;',
                'frame': '&#61515;',
                'drawer': '&#61522;',
                'rss': '&#61524;',
                'path': '&#61528;',
                'cart': '&#61530;',
                'shareable': '&#61529;',
                'login': '&#61533;',
                'logout': '&#61534;',
                'volume': '&#61538;',
                'expand': '&#61546;',
                'reduce': '&#61547;',
                'new-tab': '&#61548;',
                'publish': '&#61549;',
                'browser': '&#61550;',
                'arrow-combo': '&#61551;',
                'pie-chart': '&#61637;',
                'language': '&#61643;',
                'air': '&#61647;',
                'database': '&#61652;',
                'drive': '&#61653;',
                'bucket': '&#61654;',
                'thermometer': '&#61655;',
                'down-round': '&#61552;',
                'left-round': '&#61553;',
                'right-round': '&#61554;',
                'up-round': '&#61555;',
                'chevron-down': '&#61556;',
                'chevron-left': '&#61557;',
                'chevron-right': '&#61558;',
                'chevron-up': '&#61559;',
                'chevron-small-down': '&#61560;',
                'chevron-small-left': '&#61561;',
                'chevron-small-right': '&#61562;',
                'chevron-small-up': '&#61563;',
                'chevron-thin-down': '&#61564;',
                'chevron-thin-left': '&#61565;',
                'chevron-thin-right': '&#61566;',
                'chevron-thin-up': '&#61567;',
                'progress-0': '&#61600;',
                'progress-1': '&#61601;',
                'progress-2': '&#61602;',
                'progress-3': '&#61603;',
                'back-in-time': '&#61611;',
                'network': '&#61614;',
                'mailbox': '&#61616;',
                'download': '&#61617;',
                'buoy': '&#61626;',
                'tag': '&#61627;',
                'dot': '&#61630;',
                'two-dots': '&#61631;',
                'ellipsis': '&#61632;',
                'suitcase': '&#61629;',
                'flow-cascade': '&#61657;',
                'flow-branch': '&#61658;',
                'flow-tree': '&#61659;',
                'flow-line': '&#61660;',
                'flow-parallel': '&#61661;',
                'brush': '&#61633;',
                'paper-plane': '&#61624;',
                'magnet': '&#61634;',
                'gauge': '&#61663;',
                'traffic-cone': '&#61664;',
                'icon-creative-commons': '&#61665;',
                'cc-by': '&#61666;',
                'cc-nc': '&#61667;',
                'cc-nc-eu': '&#61668;',
                'cc-nc-jp': '&#61669;',
                'cc-sa': '&#61670;',
                'cc-nd': '&#61671;',
                'cc-pd': '&#61672;',
                'cc-zero': '&#61673;',
                'cc-share': '&#61674;',
                'cc-remix': '&#61675;',
                'github': '&#61676;',
                'github-circled': '&#61677;',
                'flickr': '&#61678;',
                'flickr-circled': '&#61679;',
                'vimeo': '&#61680;',
                'vimeo-circled': '&#61681;',
                'twitter': '&#61682;',
                'twitter-circled': '&#61683;',
                'facebook': '&#61684;',
                'facebook-circled': '&#61685;',
                'facebook-squared': '&#61686;',
                'gplus': '&#61687;',
                'gplus-circled': '&#61688;',
                'pinterest': '&#61689;',
                'pinterest-circled': '&#61690;',
                'tumblr': '&#61691;',
                'tumblr-circled': '&#61692;',
                'linkedin': '&#61693;',
                'linkedin-circled': '&#61694;',
                'dribbble': '&#61695;',
                'dribbble-circled': '&#61696;',
                'stumbleupon': '&#61697;',
                'stumbleupon-circled': '&#61698;',
                'lastfm': '&#61699;',
                'lastfm-circled': '&#61700;',
                'rdio': '&#61701;',
                'rdio-circled': '&#61702;',
                'spotify': '&#61703;',
                'spotify-circled': '&#61704;',
                'qq': '&#61705;',
                'instagrem': '&#61706;',
                'dropbox': '&#61707;',
                'evernote': '&#61708;',
                'flattr': '&#61709;',
                'skype': '&#61710;',
                'skype-circled': '&#61711;',
                'renren': '&#61712;',
                'sina-weibo': '&#61713;',
                'paypal': '&#61714;',
                'picasa': '&#61715;',
                'soundcloud': '&#61716;',
                'mixi': '&#61717;',
                'behance': '&#61718;',
                'google-circles': '&#61719;',
                'vkontakte': '&#61720;',
                'smashing': '&#61721;',
                'db-shape': '&#61723;',
                'bullet-list': '&#61722;',
                'db-logo': '&#61724;',
                'pictures': '&#61454;',
                'globe': '&#61618;',
                'leaf': '&#61625;',
                'graduation-cap': '&#61642;',
                'mic': '&#61535;',
                'palette': '&#61605;',
                'ticket': '&#61644;',
                'movie': '&#61453;',
                'target': '&#61604;',
                'music': '&#61442;',
                'trophy': '&#61609;',
                'like': '&#61484;',
                'unlike': '&#61485;',
                'bag': '&#61531;',
                'user': '&#61450;',
                'users': '&#61451;',
                'light-bulb': '&#61541;',
                'new': '&#61504;',
                'water': '&#61645;',
                'droplet': '&#61646;',
                'credit-card': '&#61648;',
                'monitor': '&#61612;',
                'briefcase': '&#61628;',
                'save': '&#61649;',
                'cd': '&#61615;',
                'folder': '&#61521;',
                'page-list': '&#61516;',
                'calendar': '&#61532;',
                'line-graph': '&#61638;',
                'bar-graph': '&#61639;',
                'clipboard': '&#61650;',
                'paperclip': '&#61476;',
                'ribbons': '&#61482;',
                'book': '&#61520;',
                'read': '&#61519;',
                'phone': '&#61525;',
                'megaphone': '&#61651;',
                'outbox': '&#61487;',
                'inbox': '&#61486;',
                'box': '&#61523;',
                'newspaper': '&#61518;',
                'mobile': '&#61613;',
                'wifi': '&#61608;',
                'camera': '&#61455;',
                'swap': '&#61589;',
                'loop': '&#61590;',
                'cycle': '&#61586;',
                'light-down': '&#61542;',
                'light-up': '&#61543;',
                'mute': '&#61536;',
                'loud': '&#61537;',
                'battery': '&#61610;',
                'search': '&#61443;',
                'key': '&#61656;',
                'lock': '&#61477;',
                'unlock': '&#61478;',
                'bell': '&#61502;',
                'ribbon': '&#61481;',
                'link': '&#61475;',
                'revert': '&#61473;',
                'flashlight': '&#61444;',
                'area-graph': '&#61640;',
                'clock': '&#61539;',
                'rocket': '&#61662;',
                'forbidden': '&#61545;'
            };
            /*this.findIn(self, children, '[class^="icon-"],[class*=" icon-"]').each(function(i) {
                var name = /icon-([^ ]+)/.exec(this.className)[1],
                    element = $(this);
                if (iconMap[name]) {
                    element.children('.icon-font:first').remove();
                    element.prepend('<span class="font-icon' + (element.is(':empty') ? ' empty': '') + '">' + iconMap[name] + '</span>');
                }
            });*/
        }
        if ($.template.ie7) {
            var pseudo = {
                    '.bullet-list > li': {
                        before: '<span class="bullet-list-before">&#61456;</span>'
                    },
                    '.info-bubble': {
                        before: '<span class="info-bubble-before"></span>'
                    },
                    '.select-arrow': {
                        before: '<span class="select-arrow-before"></span>',
                        after: '<span class="select-arrow-after"></span>'
                    },
                    '.with-left-arrow, .with-right-arrow, .tabs > li > a': {
                        after: '<span class="with-arrow-after"></span>'
                    },
                    '#menu': {
                        before: '<span id="menu-before"></span>',
                        after: '<span id="menu-after"></span>'
                    },
                    '.number-up, .number-down': {
                        after: '<span class="number-after"></span>'
                    }
                },
                target = this;
            this.findIn(self, children, 'ul, li, dd, p, fieldset, .fieldset, button, .button, input, .input-info, .field-drop, .select, .loader').filter(':last-child').addClass('last-child');
            $.each(pseudo,
                function(key, value) {
                    var elements = target.findIn(self, children, key);
                    if (elements.length > 0) {
                        if (value.before) {
                            elements.prepend(value.before);
                        }
                        if (value.after) {
                            elements.append(value.after);
                        }
                    }
                });
            var buttonIcons = this.findIn(self, children, '.button-icon');
            buttonIcons.not('.right-side').parent().css('padding-left', '0px').css('border-left', '0');
            buttonIcons.filter('.right-side').before('&nbsp;&nbsp;').parent().css('padding-right', '0px').css('border-right', '0');
            var buttons = this.findIn(self, children, '.input').children('.button').not('.compact');
            buttons.each(function(i) {
                if (!this.previousSibling) {
                    $(this).parent().css('padding-left', '0px');
                }
                else if (!this.nextSibling) {
                    $(this).parent().css('padding-right', '0px');
                }
            });
            this.findIn(self, children, '.icon > img, .stack, .controls > :first-child').before('<span class="vert-align">&nbsp;</span>');
        }
        if ($.template.ie8) {
            this.findIn(self, children, 'ul, li, dd, p, fieldset, .fieldset, button, .button, input, .input-info, .field-drop, .select, .loader').filter(':last-child').addClass('last-child');
            this.findIn(self, children, '[class^="icon-"],[class*=" icon-"]').each(function(i) {
                var element = $(this);
                if (element.is(':empty')) {
                    element.addClass('font-icon-empty');
                }
            });
        }
        return this;
    });
    $.template.addClearFunction(function(self, children) {
        var elements = this;
        if (self) {
            elements.filter('.replacement').each(function(i) {
                var replaced = $(this).data('replaced');
                if (replaced) {
                    elements = elements.add(replaced);
                }
            });
        }
        elements.findIn(self, children, '.tracking').stopTracking().remove();
        elements.findIn(self, children, '.tracked').getTrackers().stopTracking().remove();
        elements.findIn(self, children, '.withClearFunctions').each(function(i) {
            var target = this,
                element = $(target),
                functions = element.data('clearFunctions') || [];
            $.each(functions,
                function(i) {
                    this.apply(target);
                });
            element.removeClass('withClearFunctions').removeData('clearFunctions');
        });
        return elements;
    });
    function _refreshMediaQueriesInfo(triggerEvents) {
        if (!init) {
            return false;
        }
        var isWatching = $.template.disableDOMWatch(),
            test = $('<div id="mediaquery-checker"></div>').appendTo(bod),
            width = test.width(),
            height = test.height(),
            previousName = $.template.mediaQuery.name,
            changed,
            previousGroup,
            newGroup;
        test.remove();
        if (isWatching) {
            $.template.enableDOMWatch();
        }
        $.template.mediaQuery.on = [];
        $.each(mediaQueries,
            function(index, value) {
                $.template.mediaQuery.on.push(value[1]);
                if (width <= value[0]) {
                    $.template.mediaQuery.name = value[1];
                    return false;
                }
            });
        $.template.mediaQuery.hires = (height >= hiresTestHeight);
        changed = (previousName != $.template.mediaQuery.name);
        if (changed && triggerEvents) {
            if (previousName.indexOf('-') > -1) {
                previousGroup = previousName.split('-').shift();
            }
            if ($.template.mediaQuery.name.indexOf('-') > -1) {
                newGroup = $.template.mediaQuery.name.split('-').shift();
            }
            doc.trigger('quit-query-' + previousName);
            if (previousGroup && (!newGroup || newGroup != previousGroup)) {
                doc.trigger('quit-query-' + previousGroup);
            }
            doc.trigger('change-query');
            if (newGroup && (!previousGroup || previousGroup != newGroup)) {
                doc.trigger('enter-query-' + newGroup);
            }
            doc.trigger('enter-query-' + $.template.mediaQuery.name);
        }
        return changed;
    }
    function handleResize() {
        $.template.viewportWidth = win.width();
        $.template.viewportHeight = $.template.iPhone ? window.innerHeight: win.height();
        win.trigger('normalized-preresize');
        _refreshMediaQueriesInfo(true);
        bod.refreshInnerTrackedElements();
        win.trigger('normalized-resize');
        resizeInt = false;
    }
    win.on('resize',
        function() {
            if (!resizeInt && $.isReady) {
                resizeInt = setTimeout(handleResize, 40);
            }
        }).on('orientationchange', handleResize);
    doc.on('respond-ready',
        function() {
            _refreshMediaQueriesInfo(true);
        });
    $.template.init = function() {
        var menu = $('#menu'),
            menuContent = $('#menu-content'),
            previousScroll = false;
//            watchMenuSize;
        if (init) {
            return;
        }
        init = true;
        _refreshMediaQueriesInfo(false);
        bod.applySetup();
        doc.trigger('init-queries');
        doc.trigger('enter-query-' + $.template.mediaQuery.name);
        $('#open-menu').on('touchend click',
            function(event) {
                event.preventDefault();
                if (!$.template.processTouchClick(this, event)) {
                    return;
                }
                bod.removeClass('shortcuts-open');
                $("#tree > div , #tree > div .bui-grid-header ,#tree > div .bui-grid-body,#tree > div .bui-grid-body > table").css("width", "100%");
                bod.toggleClass($.template.mediaQuery.is('desktop') || $.template.mediaQuery.is('tablet-landscape') ? 'menu-hidden': 'menu-open');
                if ($.template.mediaQuery.is('mobile') && bod.hasClass('menu-open') && bod.hasClass('fixed-title-bar')) {
                    previousScroll = bod.scrollTop();
                    bod.removeClass('fixed-title-bar');
                    bod.scrollTop(0);
                   
                }
                else if (previousScroll !== false) {
                    if ($.template.mediaQuery.is('mobile')) {
                        bod.scrollTop(previousScroll);
                    }
                    previousScroll = false;
                    bod.addClass('fixed-title-bar');
                   
                }
                watchMenuSize();
            });
        bod.children().on('click',
            function(event) {
                if (bod.hasClass('menu-open') && !$(event.target).closest('#open-menu, #menu').length) {
                    if (previousScroll !== false) {
                        if ($.template.mediaQuery.is('mobile')) {
                            bod.scrollTop(previousScroll);
                        }
                        previousScroll = false;
                        bod.addClass('fixed-title-bar');
                    }
                    bod.removeClass('menu-open');
                }
            });
        $('#open-shortcuts').on('touchend click',
            function(event) {
                event.preventDefault();
                if (!$.template.processTouchClick(this, event)) {
                    return;
                }
                if (previousScroll !== false && bod.hasClass('menu-open')) {
                    if ($.template.mediaQuery.is('mobile')) {
                        bod.scrollTop(previousScroll);
                    }
                    previousScroll = false;
                    bod.addClass('fixed-title-bar');
                }
                bod.removeClass('menu-open').toggleClass('shortcuts-open');
            });
        watchMenuSize = function() {
            if (!bod.hasClass('menu-open') || !$.template.mediaQuery.is('tablet-portrait')) {
                menuContent.css('max-height', '');
                return;
            }
            var siblingsHeight = 0;
            menuContent.siblings().each(function(i) {
                siblingsHeight += $(this).outerHeight();
            });
            menuContent.css('max-height', (Math.round(0.9 * $.template.viewportHeight) - (menu.outerHeight() - menu.height()) - siblingsHeight) + 'px');
        };
        watchMenuSize();
        win.on('normalized-resize', watchMenuSize);
        if ($.fn.customScroll) {
            var scrollMenu = false,
                scrollContent = false,
                updateMenuScroll = function() {
                    if ($.template.mediaQuery.isSmallerThan('tablet-portrait')) {
                        if (scrollMenu) {
                            menu.removeCustomScroll();
                            scrollMenu = false;
                        }
                        if (scrollContent) {
                            menuContent.removeCustomScroll();
                            scrollContent = false;
                        }
                    }
                    else if ($.template.mediaQuery.is('tablet-portrait')) {
                        if (scrollMenu) {
                            menu.removeCustomScroll();
                            scrollMenu = false;
                        }
                        if (!scrollContent) {
                            menuContent.customScroll();
                            scrollContent = true;
                        }
                    }
                    else {
                        if (scrollContent) {
                            menuContent.removeCustomScroll();
                            scrollContent = false;
                        }
                        if (!scrollMenu) {
                            menu.customScroll();
                            scrollMenu = true;
                        }
                    }
                };
            updateMenuScroll();
            doc.on('change-query', updateMenuScroll);
        }
        if (('standalone' in window.navigator) && window.navigator.standalone) {
            doc.on('click', 'body',
                function(event) {
                    var link = $(event.target).closest('a'),
                        href;
                    if (!link.length) {
                        return;
                    }
                    if (event.isDefaultPrevented()) {
                        return;
                    }
                    href = link.attr('href');
                    if (!href || href.indexOf('#') === 0) {
                        return;
                    }
                    if (link.hasClass('navigable-ajax') || link.hasClass('navigable-ajax-loaded')) {
                        return;
                    }
                    if (! (/^[a-z+\.\-]+:/i).test(href) || href.indexOf(document.location.protocol + '//' + document.location.host) === 0) {
                        event.preventDefault();
                        document.location.href = href;
                    }
                });
        }
    };
    doc.ready(function() {
        $.template.init();
    });
    doc.on('click', '.close',
        function(event) {
            var close = $(this),
                parent = close.parent();
            event.preventDefault();
            close.remove();
            parent.addClass('closing').fadeAndRemove().trigger('close');
        });
    if (Modernizr.touch) {
        doc.on('touchend', '.info-spot',
            function(event) {
                if (!$.template.processTouchClick(this, event)) {
                    return;
                }
                var info = $(this),
                    content = info.children('.info-bubble').html();
                if (content && content.length > 0) {
                    event.preventDefault();
                    if ($.modal) {
                        $.modal.alert(content);
                    }
                    else {
                        alert(content);
                    }
                }
            });
    }
    else {
        doc.on('mouseenter', '.info-spot',
            function(event) {
                var info = $(this),
                    bubble = info.children('.info-bubble');
                if (info.hasClass('on-left')) {
                    if (bubble.offset().left < 0) {
                        info.removeClass('on-left').data('info-spot-reverse-x', true);
                    }
                }
                else {
                    if (bubble.offset().left + bubble.outerWidth() > $.template.viewportWidth) {
                        info.addClass('on-left').data('info-spot-reverse-x', true);
                    }
                }
                if (info.hasClass('on-top')) {
                    if (bubble.offset().top < doc.scrollTop()) {
                        info.removeClass('on-top').data('info-spot-reverse-y', true);
                    }
                }
                else {
                    if (bubble.offset().top + bubble.outerHeight() > doc.scrollTop() + $.template.viewportHeight) {
                        info.addClass('on-top').data('info-spot-reverse-y', true);
                    }
                }
            }).on('mouseleave', '.info-spot',
            function(event) {
                var info = $(this);
                if (info.data('info-spot-reverse-x')) {
                    info.toggleClass('on-left');
                    info.removeData('info-spot-reverse-x');
                }
                if (info.data('info-spot-reverse-y')) {
                    info.toggleClass('on-top');
                    info.removeData('info-spot-reverse-y');
                }
            });
    }
    if (!Modernizr.pointerevents) {
        doc.on('click mouseover', '.no-pointer-events',
            function(event) {
                this.style.display = 'none';
                var x = event.pageX,
                    y = event.pageY,
                    under = document.elementFromPoint(x, y);
                this.style.display = '';
                event.stopPropagation();
                event.preventDefault();
                $(under).trigger(event.type);
            });
    }
    $.fn.trackElement = function(target, refreshFunc) {
        target = target.eq(0).addClass('tracked');
        if (!refreshFunc) {
            refreshFunc = function(target) {
                $(this).offset(target.offset());
            };
        }
        var targetDOM = target[0],
            tracking = target.data('tracking-elements') || [];
        this.css({
            position: 'absolute'
        }).addClass('tracking').each(function(i) {
                var element = $(this),
                    tracked = element.data('tracked-element');
                if (tracked && tracked !== targetDOM) {
                    element.stopTracking();
                    tracked = null;
                }
                if (!tracked) {
                    element.data('tracked-element', targetDOM);
                    tracking.push({
                        element: this,
                        func: refreshFunc
                    });
                    refreshFunc.call(this, target);
                }
            });
        target.data('tracking-elements', tracking);
        return this;
    };
    $.fn.stopTracking = function(clearPos) {
        this.each(function() {
            var element = $(this),
                tracked = element.data('tracked-element'),
                target,
                tracking,
                i;
            if (tracked) {
                target = $(tracked);
                tracking = target.data('tracking-elements') || [];
                for (i = 0; i < tracking.length; ++i) {
                    if (tracking[i].element === this) {
                        tracking.splice(i, 1); --i;
                    }
                }
                if (tracking.length === 0) {
                    target.removeClass('tracked').removeData('tracking-elements');
                }
                else {
                    target.data('tracking-elements', tracking);
                }
                element.removeClass('tracking').removeData('tracked-element');
                element.css({
                    position: ''
                });
                if (clearPos) {
                    element.css({
                        top: '',
                        left: ''
                    });
                }
            }
        });
        return this;
    };
    $.fn.refreshTrackedElements = function() {
        this.filter('.tracked').each(function(i) {
            var target = $(this);
            $.each(target.data('tracking-elements') || [],
                function(i) {
                    $(this.element).stop(true, true);
                    this.func.call(this.element, target);
                });
        });
        return this;
    };
    win.scroll(function() {
        bod.refreshInnerTrackedElements();
    });
    $.fn.refreshInnerTrackedElements = function() {
        this.find('.tracked').each(function(i) {
            var target = $(this);
            $.each(target.data('tracking-elements') || [],
                function(i) {
                    $(this.element).stop(true, true);
                    this.func.call(this.element, target);
                });
        });
        return this;
    };
    $.fn.getTrackers = function() {
        var list = [];
        $.each($(this).data('tracking-elements') || [],
            function(i) {
                list.push(this.element);
            });
        return $(list);
    };
    $.fn.foldAndRemove = function(duration, callback) {
        $(this).slideUp(duration,
            function() {
                if (callback) {
                    callback.apply(this);
                }
                $(this).remove();
            });
        return this;
    };
    $.fn.fadeAndRemove = function(duration, callback) {
        this.animate({
                'opacity': 0
            },
            {
                'duration': duration,
                'complete': function() {
                    var element = $(this).trigger('endfade');
                    if (element.css('position') == 'absolute') {
                        if (callback) {
                            callback.apply(this);
                        }
                        element.remove();
                    }
                    else {
                        element.slideUp(duration,
                            function() {
                                if (callback) {
                                    callback.apply(this);
                                }
                                element.remove();
                            });
                    }
                }
            });
        return this;
    };
    $.fn.shake = function(force, callback) {
        force = force || 15;
        this.each(function() {
            var element = $(this),
                leftMargin = element.parseCSSValue('margin-left'),
                rightMargin = element.parseCSSValue('margin-right'),
                steps = [force, Math.round(force * 0.8), Math.round(force * 0.6), Math.round(force * 0.4), Math.round(force * 0.2)],
                effectMargins = [[leftMargin - steps[0], rightMargin + steps[0]], [leftMargin + steps[1], rightMargin - steps[1]], [leftMargin - steps[2], rightMargin + steps[2]], [leftMargin + steps[3], rightMargin - steps[3]], [leftMargin - steps[4], rightMargin + steps[4]], [leftMargin, leftMargin]];
            $.each(effectMargins,
                function(i) {
                    var options = {
                        duration: (i === 0) ? 40: 80
                    };
                    if (i === 5) {
                        options.complete = function() {
                            $(this).css({
                                marginLeft: '',
                                marginRight: ''
                            });
                            if (callback) {
                                callback.apply(this);
                            }
                        };
                    }
                    element.animate({
                            marginLeft: this[0] + 'px',
                            marginRight: this[1] + 'px'
                        },
                        options);
                });
        });
        return this;
    };
    if (!location.hash) {
        window.scrollTo(0, 1);
        var scrollTop = 1,
            getScrollTop = function() {
                return window.pageYOffset || document.compatMode === 'CSS1Compat' && document.documentElement.scrollTop || document.body.scrollTop || 0;
            },
            bodycheck = setInterval(function() {
                    if (document.body) {
                        clearInterval(bodycheck);
                        scrollTop = getScrollTop();
                        window.scrollTo(0, scrollTop === 1 ? 0: 1);
                    }
                },
                15);
        win.on('load',
            function() {
                setTimeout(function() {
                        if (getScrollTop() < 20) {
                            window.scrollTo(0, scrollTop === 1 ? 0: 1);
                        }
                    },
                    0);
            });
    }
    if ($.easing.easeOutQuad === undefined) {
        $.easing.jswing = $.easing.swing;
        $.extend($.easing, {
            def: 'easeOutQuad',
            swing: function(x, t, b, c, d) {
                return $.easing[$.easing.def](x, t, b, c, d);
            },
            easeInQuad: function(x, t, b, c, d) {
                return c * (t /= d) * t + b;
            },
            easeOutQuad: function(x, t, b, c, d) {
                return - c * (t /= d) * (t - 2) + b;
            },
            easeInOutQuad: function(x, t, b, c, d) {
                if ((t /= d / 2) < 1) {
                    return c / 2 * t * t + b;
                }
                return - c / 2 * ((--t) * (t - 2) - 1) + b;
            }
        });
    }
    var types = ['DOMMouseScroll', 'mousewheel'];
    function mouseWheelHandler(event) {
        var sentEvent = event || window.event,
            orgEvent = sentEvent.originalEvent || sentEvent,
            args = [].slice.call(arguments, 1),
            delta = 0,
            deltaX = 0,
            deltaY = 0;
        event = $.event.fix(orgEvent);
        event.type = "mousewheel";
        if (orgEvent.wheelDelta) {
            delta = orgEvent.wheelDelta / 120;
        }
        if (orgEvent.detail) {
            delta = -orgEvent.detail / 3;
        }
        deltaY = delta;
        if (orgEvent.axis !== undefined && orgEvent.axis === orgEvent.HORIZONTAL_AXIS) {
            deltaY = 0;
            deltaX = -1 * delta;
        }
        if (orgEvent.wheelDeltaY !== undefined) {
            deltaY = orgEvent.wheelDeltaY / 120;
        }
        if (orgEvent.wheelDeltaX !== undefined) {
            deltaX = -1 * orgEvent.wheelDeltaX / 120;
        }
        args.unshift(event, delta, deltaX, deltaY);
        return ($.event.dispatch || $.event.handle).apply(this, args);
    }
    $.event.special.mousewheel = {
        setup: function() {
            if (this.addEventListener) {
                for (var i = types.length; i;) {
                    this.addEventListener(types[--i], mouseWheelHandler, false);
                }
            }
            else {
                this.onmousewheel = mouseWheelHandler;
            }
        },
        teardown: function() {
            if (this.removeEventListener) {
                for (var i = types.length; i;) {
                    this.removeEventListener(types[--i], mouseWheelHandler, false);
                }
            }
            else {
                this.onmousewheel = null;
            }
        }
    };
    $.fn.extend({
        mousewheel: function(fn) {
            return fn ? this.on("mousewheel", fn) : this.trigger("mousewheel");
        },
        unmousewheel: function(fn) {
            return this.off("mousewheel", fn);
        }
    });


    $("#slidelist").click(function() {
            $(".content-panel").toggleClass("mac-hide-all");
            $(".panel-navigation").toggleClass("mac-hide")
        }
    );
    $("input.valid").after("<span style='color:  #d52607;padding-left: 8px;font-size: 14px'>*</span>");
})(this.jQuery, window, document);